using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using Excel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEditor;

public class ConfigData
{
    public string Type;
    public string Name;
    public string Bewrite;
    public string Data;
}
public class ExcelTool
{
    private static int PROPERTY_Type_LINE = 1;

    public static int PROPERTY_Name_LINE = 2;
    public static int PROPERTY_Bewrite_LINE = 3;

    public static IExcelDataReader excelReader;

    public static string duTableurl = Application.dataPath + "/ExcelTools/xlsx/";
    public static string cunbytesurl = Application.dataPath + "/Resources/TableBytes/";
    public static string cuncsurl = Application.dataPath + "/Resources/Scripts/TableData/";
    /// <summary>
    /// 载入一个excel文件
    /// </summary>
    /// <param name="filename">Filename.</param>
    public static string LoadData(string filename)
    {
        FileStream stream = File.Open(duTableurl + filename, FileMode.Open, FileAccess.Read);
        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();
        HandleATable(result.Tables[0], excelReader.Name);
        return excelReader.Name;
    }

    /// <summary>
    /// 处理一张表 Handle A table.
    /// </summary>
    /// <param name="result">Result.</param>
    public static bool HandleATable(DataTable result, string name)
    {
        string[] types = null;
        string[] names = null;
        string[] bewrites = null;
        List<ConfigData[]> dataList = new List<ConfigData[]>();
        int index = 1;

        //开始读取
        while (excelReader.Read())
        {
            //这里读取的是每一行的数据
            string[] datas = new string[excelReader.FieldCount];
            for (int j = 0; j < excelReader.FieldCount; ++j)
            {
                datas[j] = excelReader.GetString(j);
            }
            //空行不处理
            if (datas.Length == 0 || string.IsNullOrEmpty(datas[0]))
            {
                ++index;
                continue;
            }
            //第几行表示类型
            if (index == PROPERTY_Type_LINE) types = datas;
            //第几行表示变量名
            else if (index == PROPERTY_Name_LINE) names = datas;
            //第几行表示描述
            else if (index == PROPERTY_Bewrite_LINE) bewrites = datas;
            //后面的表示数据
            else if (index > PROPERTY_Bewrite_LINE)
            {
                //把读取的数据和数据类型,名称保存起来,后面用来动态生成类
                List<ConfigData> configDataList = new List<ConfigData>();
                for (int j = 0; j < datas.Length; ++j)
                {
                    ConfigData data = new ConfigData();
                    data.Type = types[j];
                    data.Name = names[j];
                    data.Data = datas[j];
                    data.Bewrite = bewrites[j];
                    if (string.IsNullOrEmpty(data.Type) || string.IsNullOrEmpty(data.Data))
                        continue;
                    configDataList.Add(data);
                }
                dataList.Add(configDataList.ToArray());
            }
            ++index;
        }
        string className = name;
        //根据刚才的数据来生成C#脚本
        ScriptGenerator generator = new ScriptGenerator(className, names, types, bewrites, cuncsurl);
        generator.Generate();
        AssetDatabase.Refresh();
        Type temp = Type.GetType(className);
        //    Assembly assembly = Assembly.GetAssembly(temp);
        //  object container = assembly.CreateInstance(className);
        Serialize(temp, dataList, cunbytesurl);
        return true;
    }
    //序列化对象
    private static void Serialize(Type temp, List<ConfigData[]> dataList, string path)
    {
        List<object> temparr = new List<object>();
        //设置数据
        foreach (ConfigData[] datas in dataList)
        {
            object t = temp.Assembly.CreateInstance(temp.FullName);
            foreach (ConfigData data in datas)
            {
                FieldInfo info = temp.GetField(data.Name);
                switch (data.Type)
                {
                    case "string":
                        info.SetValue(t, data.Data);
                        break;
                    case "int":
                        info.SetValue(t, int.Parse(data.Data));
                        break;
                    case "float":
                        info.SetValue(t, float.Parse(data.Data));
                        break;
                    case "double":
                        info.SetValue(t, double.Parse(data.Data));
                        break;
                    case "long":
                        info.SetValue(t, long.Parse(data.Data));
                        break;
                    case "List<int>":
                        info.SetValue(t, data.Data.Split(';').Select(int.Parse).ToList());
                        break;
                    case "List<float>":
                        info.SetValue(t, data.Data.Split(';').Select(int.Parse).ToList());
                        break;
                    case "List<double>":
                        info.SetValue(t, data.Data.Split(';').Select(double.Parse).ToList());
                        break;
                    case "List<long>":
                        info.SetValue(t, data.Data.Split(';').Select(long.Parse).ToList());
                        break;
                }
            }
            temparr.Add(t);
            IFormatter f = new BinaryFormatter();
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            Stream s = new FileStream(path + temp.Name + ".bytes", FileMode.OpenOrCreate,
                FileAccess.Write, FileShare.Write);
            f.Serialize(s, temparr);
            s.Close();
        }
    }
}

public class ScriptGenerator
{
    public string[] Fileds;
    public string[] Types;
    public string[] Bewrites;
    public string ClassName;
    public string Url;
    public ScriptGenerator(string className, string[] fileds, string[] types, string[] bewrites, string url)
    {
        ClassName = className;
        Fileds = fileds;
        Types = types;
        Bewrites = bewrites;
        Url = url;
    }

    //开始生成脚本
    public string Generate()
    {
        if (Types == null || Fileds == null || ClassName == null)
            return null;
        return CreateCode(ClassName, Types, Fileds, Bewrites, Url);
    }

    //创建代码。   
    private string CreateCode(string tableName, string[] types, string[] fields, string[] bewrites, string url)
    {
        //生成类
        StringBuilder classSource = new StringBuilder();

        classSource.Append("\n");
        classSource.Append("using System;\n");
        classSource.Append("using System.Collections.Generic;\n");
        classSource.Append("using System.IO;\n");
        classSource.Append("using System.Runtime.Serialization;\n");
        classSource.Append("using System.Runtime.Serialization.Formatters.Binary;\n");
        classSource.Append("using UnityEngine;\n");
        classSource.Append("[Serializable]\n");
        classSource.Append("public class " + tableName + "\n");
        classSource.Append("{\n");
        //设置成员
        for (int i = 0; i < fields.Length; ++i)
        {
            classSource.Append("\t/// <summary>\n");
            classSource.Append("\t/// " + bewrites[i] + "\n");
            classSource.Append("\t/// </summary>\n");
            classSource.Append(PropertyString(types[i], fields[i]));
        }
        classSource.Append("}\n");

        //生成Container
        classSource.Append("\n");
        classSource.Append("[Serializable]\n");
        classSource.Append("public class " + tableName + "DataManager\n");
        classSource.Append("{\n");
        classSource.Append("  \tpublic static " + tableName + "DataManager _ins;\n");
        classSource.Append("  \tpublic static " + tableName + "DataManager Ins\n");
        classSource.Append("    {\n");
        classSource.Append("     get { return _ins ?? (_ins = new " + tableName + "DataManager()); }\n");
        classSource.Append("    }\n");
        classSource.Append("\tpublic " + "Dictionary<int, " + tableName + ">" + " Dict" + " = new Dictionary<int, " + tableName + ">();\n");

        //反序列化
        classSource.Append("\tprivate " + tableName + "DataManager()\n");
        classSource.Append("\t{\n");
        classSource.Append("\t\tIFormatter f = new BinaryFormatter();\n");
        classSource.Append("\t\tTextAsset text = Resources.Load<TextAsset>(" + '"' + "TableBytes/" + '"' + '+' + '"' + tableName + '"' + ");\n");
        classSource.Append("\t\tStream s = new MemoryStream(text.bytes);\n");
        classSource.Append("\t\tvar objarr = (List<object>)f.Deserialize(s);\n");
        classSource.Append("\t\tfor (int i = 0; i < objarr.Count; i++)\n");
        classSource.Append("\t\t{\n");
        classSource.Append("\t\t\t" + tableName + " type = (" + tableName + ") objarr[i];\n");
        classSource.Append("\t\t\t Dict.Add(type." + fields[1] + ",type);\n");
        classSource.Append("\t\t}\n");
        classSource.Append("\t\ts.Close();\n");
        classSource.Append("\t}\n");

        classSource.Append("}\n");
        //保存脚本
        if (!Directory.Exists(url)) Directory.CreateDirectory(url);
        StreamWriter sw = new StreamWriter(url + tableName + "DataManager.cs");
        sw.WriteLine(classSource.ToString());
        sw.Close();
        return classSource.ToString();
    }

    private string PropertyString(string type, string propertyName)
    {
        if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(propertyName))
            return null;
        StringBuilder sbProperty = new StringBuilder();
        sbProperty.Append("\tpublic " + type + " " + propertyName + ";\n");
        return sbProperty.ToString();
    }



}
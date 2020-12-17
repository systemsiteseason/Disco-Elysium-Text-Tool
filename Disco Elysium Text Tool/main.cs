using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Disco_Elysium_Text_Tool
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        OpenFileDialog opf = new OpenFileDialog();

        private void btnGet_Click(object sender, EventArgs e)
        {
            opf.Filter = "Database Text|sharedassets1_00001.-63;resources_00010.-2";
            if(opf.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(opf.FileName))
                {
                    if (Path.GetFileName(opf.FileName) == "resources_00010.-2")
                    {
                        BinaryReader rd = new BinaryReader(new FileStream(opf.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                        rd.BaseStream.Seek(60, SeekOrigin.Begin);
                        int count = rd.ReadInt32();
                        StreamWriter wt = new StreamWriter(Path.GetDirectoryName(opf.FileName) + "\\" + Path.GetFileName(opf.FileName) + ".txt");
                        for (int i = 0; i < count; i++)
                        {
                            int num = rd.ReadInt32();
                            string id = Encoding.UTF8.GetString(rd.ReadBytes(num));
                            SkipByte(rd);
                            rd.ReadBytes(12);
                            int size = rd.ReadInt32();
                            string[] all_lang = new string[size];
                            for(int j = 0; j < size; j++)
                            {
                                num = rd.ReadInt32();
                                string txt = Encoding.UTF8.GetString(rd.ReadBytes(num));
                                StringBuilder sb = new StringBuilder(txt);
                                sb.Replace("\r", "[r]");
                                sb.Replace("\n", "[n]");
                                sb.Replace("\"", "[0]");
                                SkipByte(rd);
                                all_lang[j] = sb.ToString();
                            }
                            rd.ReadBytes(12);
                            wt.WriteLine(id + "=" + all_lang[0]);
                        }
                        wt.Close();
                    }
                    else
                    {
                        StreamWriter wt = new StreamWriter(Path.GetDirectoryName(opf.FileName) + "\\" + Path.GetFileName(opf.FileName) + ".FileSizeTable");
                        //settings
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.ConformanceLevel = ConformanceLevel.Fragment;
                        settings.Indent = true;
                        settings.NewLineOnAttributes = ckb.Checked;

                        BinaryReader rd = new BinaryReader(new FileStream(opf.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                        rd.BaseStream.Seek(224, SeekOrigin.Begin);
                        int num = rd.ReadInt32();
                        //Actors
                        XmlWriter xmlwtactor = XmlWriter.Create(Path.GetDirectoryName(opf.FileName) + "\\Actors.xml", settings);
                        xmlwtactor.WriteStartElement("Actors");
                        xmlwtactor.WriteAttributeString("count", num.ToString());
                        wt.WriteLine(Path.GetDirectoryName(opf.FileName) + "\\Actors.xml");
                        for (int i = 0; i < num; i++)
                        {
                            int id = rd.ReadInt32();
                            xmlwtactor.WriteStartElement("Actor");
                            xmlwtactor.WriteAttributeString("Id", id.ToString());
                            int but = rd.ReadInt32();
                            for (int j = 0; j < but; j++)
                            {
                                int n = rd.ReadInt32();
                                StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                key.Replace(" ", "-");
                                SkipByte(rd);
                                n = rd.ReadInt32();
                                StringBuilder value = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                value.Replace("\r", "[r]");
                                value.Replace("\n", "[n]");
                                SkipByte(rd);
                                rd.ReadInt32();
                                n = rd.ReadInt32();
                                rd.ReadBytes(n);
                                SkipByte(rd);
                                xmlwtactor.WriteAttributeString(key.ToString(), value.ToString());
                            }
                            xmlwtactor.WriteEndElement();
                            rd.ReadBytes(32);
                        }

                        xmlwtactor.WriteEndElement();
                        xmlwtactor.Close();

                        //Items
                        num = rd.ReadInt32();
                        XmlWriter xmlwtitem = XmlWriter.Create(Path.GetDirectoryName(opf.FileName) + "\\Items.xml", settings);
                        xmlwtitem.WriteStartElement("Items");
                        xmlwtitem.WriteAttributeString("count", num.ToString());
                        wt.WriteLine(Path.GetDirectoryName(opf.FileName) + "\\Items.xml");
                        for (int i = 0; i < num; i++)
                        {
                            int id = rd.ReadInt32();
                            xmlwtitem.WriteStartElement("Item");
                            xmlwtitem.WriteAttributeString("Id", id.ToString());
                            int but = rd.ReadInt32();
                            for (int j = 0; j < but; j++)
                            {
                                int n = rd.ReadInt32();
                                StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                key.Replace(" ", "-");
                                SkipByte(rd);
                                n = rd.ReadInt32();
                                StringBuilder value = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                value.Replace("\r", "[r]");
                                value.Replace("\n", "[n]");
                                SkipByte(rd);
                                rd.ReadInt32();
                                n = rd.ReadInt32();
                                rd.ReadBytes(n);
                                SkipByte(rd);
                                xmlwtitem.WriteAttributeString(key.ToString(), value.ToString());
                            }
                            xmlwtitem.WriteEndElement();
                        }

                        xmlwtitem.WriteEndElement();
                        xmlwtitem.Close();

                        //Locations
                        num = rd.ReadInt32();
                        XmlWriter xmlwtlocation = XmlWriter.Create(Path.GetDirectoryName(opf.FileName) + "\\Locations.xml", settings);
                        xmlwtlocation.WriteStartElement("Locations");
                        xmlwtlocation.WriteAttributeString("count", num.ToString());
                        wt.WriteLine(Path.GetDirectoryName(opf.FileName) + "\\Locations.xml");
                        for (int i = 0; i < num; i++)
                        {
                            int id = rd.ReadInt32();
                            xmlwtlocation.WriteStartElement("Location");
                            xmlwtlocation.WriteAttributeString("Id", id.ToString());
                            int but = rd.ReadInt32();
                            for (int j = 0; j < but; j++)
                            {
                                int n = rd.ReadInt32();
                                StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                key.Replace(" ", "-");
                                SkipByte(rd);
                                n = rd.ReadInt32();
                                StringBuilder value = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                value.Replace("\r", "[r]");
                                value.Replace("\n", "[n]");
                                SkipByte(rd);
                                rd.ReadInt32();
                                n = rd.ReadInt32();
                                rd.ReadBytes(n);
                                SkipByte(rd);
                                xmlwtlocation.WriteAttributeString(key.ToString(), value.ToString());
                            }
                            xmlwtlocation.WriteEndElement();
                        }

                        xmlwtlocation.WriteEndElement();
                        xmlwtlocation.Close();

                        //Variables
                        num = rd.ReadInt32();
                        XmlWriter xmlwtvariable = XmlWriter.Create(Path.GetDirectoryName(opf.FileName) + "\\Variables.xml", settings);
                        xmlwtvariable.WriteStartElement("Variables");
                        xmlwtvariable.WriteAttributeString("count", num.ToString());
                        wt.WriteLine(Path.GetDirectoryName(opf.FileName) + "\\Variables.xml");
                        for (int i = 0; i < num; i++)
                        {
                            int id = rd.ReadInt32();
                            xmlwtvariable.WriteStartElement("Variable");
                            xmlwtvariable.WriteAttributeString("Id", id.ToString());
                            int but = rd.ReadInt32();
                            for (int j = 0; j < but; j++)
                            {
                                int n = rd.ReadInt32();
                                StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                key.Replace(" ", "-");
                                SkipByte(rd);
                                n = rd.ReadInt32();
                                StringBuilder value = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                value.Replace("\r", "[r]");
                                value.Replace("\n", "[n]");
                                SkipByte(rd);
                                rd.ReadInt32();
                                n = rd.ReadInt32();
                                rd.ReadBytes(n);
                                SkipByte(rd);
                                xmlwtvariable.WriteAttributeString(key.ToString(), value.ToString());
                            }
                            xmlwtvariable.WriteEndElement();
                        }

                        xmlwtvariable.WriteEndElement();
                        xmlwtvariable.Close();

                        //Conversations
                        num = rd.ReadInt32();
                        XmlWriter xmlwtconversation = XmlWriter.Create(Path.GetDirectoryName(opf.FileName) + "\\Conversations.xml", settings);
                        xmlwtconversation.WriteStartElement("Conversations");
                        xmlwtconversation.WriteAttributeString("count", num.ToString());
                        wt.WriteLine(Path.GetDirectoryName(opf.FileName) + "\\Conversations.xml");
                        for (int i = 0; i < num; i++)
                        {
                            int id = rd.ReadInt32();
                            xmlwtconversation.WriteStartElement("Conversation");
                            xmlwtconversation.WriteAttributeString("Id", id.ToString());
                            int but = rd.ReadInt32();
                            for (int j = 0; j < but; j++)
                            {
                                int n = rd.ReadInt32();
                                StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                key.Replace(" ", "-");
                                SkipByte(rd);
                                n = rd.ReadInt32();
                                StringBuilder value = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                value.Replace("\r", "[r]");
                                value.Replace("\n", "[n]");
                                SkipByte(rd);
                                rd.ReadInt32();
                                n = rd.ReadInt32();
                                rd.ReadBytes(n);
                                SkipByte(rd);
                                xmlwtconversation.WriteAttributeString(key.ToString(), value.ToString());
                            }
                            rd.ReadBytes(40);
                            but = rd.ReadInt32();
                            rd.ReadBytes(but);
                            SkipByte(rd);
                            but = rd.ReadInt32();
                            rd.ReadBytes(but);
                            SkipByte(rd);
                            but = rd.ReadInt32();
                            rd.ReadBytes(but);
                            SkipByte(rd);
                            rd.ReadBytes(16);
                            but = rd.ReadInt32();
                            rd.ReadBytes(but);
                            SkipByte(rd);
                            but = rd.ReadInt32();
                            for(int j = 0; j < but; j++)
                            {
                                id = rd.ReadInt32();
                                xmlwtconversation.WriteStartElement("DialogueEntry");
                                xmlwtconversation.WriteAttributeString("Id", id.ToString());
                                int kbut = rd.ReadInt32();
                                for(int k = 0; k < kbut; k++)
                                {
                                    int n = rd.ReadInt32();
                                    StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                    key.Replace(" ", "-");
                                    SkipByte(rd);
                                    n = rd.ReadInt32();
                                    StringBuilder value = new StringBuilder(Encoding.UTF8.GetString(rd.ReadBytes(n)));
                                    value.Replace("\r", "[r]");
                                    value.Replace("\n", "[n]");
                                    SkipByte(rd);
                                    rd.ReadInt32();
                                    n = rd.ReadInt32();
                                    rd.ReadBytes(n);
                                    SkipByte(rd);
                                    xmlwtconversation.WriteAttributeString(key.ToString(), value.ToString());
                                }
                                rd.ReadBytes(12);
                                kbut = rd.ReadInt32();
                                rd.ReadBytes(kbut);
                                SkipByte(rd);
                                rd.ReadInt32();
                                kbut = rd.ReadInt32();
                                rd.ReadBytes(kbut);
                                SkipByte(rd);
                                rd.ReadInt32();
                                kbut = rd.ReadInt32();
                                rd.ReadBytes(kbut * 24);
                                kbut = rd.ReadInt32();
                                rd.ReadBytes(kbut);
                                SkipByte(rd);
                                kbut = rd.ReadInt32();
                                rd.ReadBytes(kbut);
                                SkipByte(rd);
                                kbut = rd.ReadInt32();
                                rd.ReadBytes(kbut * 24);
                                rd.ReadBytes(16);
                                xmlwtconversation.WriteEndElement();
                            }
                            rd.ReadBytes(12);
                            xmlwtconversation.WriteEndElement();
                        }
                        
                        xmlwtconversation.WriteEndElement();
                        xmlwtconversation.Close();

                        wt.Close();
                    }
                    
                    if (!File.Exists(opf.FileName + "_bk"))
                        Copy(opf.FileName, opf.FileName + "_bk");

                    MessageBox.Show("Done!!");
                }
            }
        }

        private void btnCook_Click(object sender, EventArgs e)
        {
            opf.Filter = "Database Text|sharedassets1_00001.-63.FileSizeTable;resources_00010.-2.txt";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(opf.FileName))
                {
                    if (Path.GetFileName(opf.FileName) == "resources_00010.-2.txt")
                    {
                        string[] lines = File.ReadAllLines(opf.FileName);
                        BinaryReader rd = new BinaryReader(new FileStream(Path.GetDirectoryName(opf.FileName) + "\\" + Path.GetFileNameWithoutExtension(opf.FileName) + "_bk", FileMode.Open, FileAccess.Read, FileShare.Read));
                        BinaryWriter wt = new BinaryWriter(new FileStream(Path.GetDirectoryName(opf.FileName) + "\\" + Path.GetFileNameWithoutExtension(opf.FileName), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite));
                        wt.Write(rd.ReadBytes(60));
                        int count = rd.ReadInt32();
                        wt.Write(count);
                        foreach (string line in lines)
                        {
                            int num = rd.ReadInt32();
                            wt.Write(num);
                            wt.Write(rd.ReadBytes(num));
                            SkipByte(rd);
                            SkipWrite(wt);
                            wt.Write(rd.ReadBytes(12));
                            int size = rd.ReadInt32();
                            wt.Write(size);
                            for (int j = 0; j < size; j++)
                            {
                                if(j == 0)
                                {
                                    num = rd.ReadInt32();
                                    rd.ReadBytes(num);
                                    StringBuilder sb = new StringBuilder(line.Split('=')[1]);
                                    sb.Replace("[r]", "\r");
                                    sb.Replace("[n]", "\n");
                                    sb.Replace("[0]", "\"");
                                    wt.Write(Encoding.UTF8.GetBytes(sb.ToString()).Length);
                                    wt.Write(Encoding.UTF8.GetBytes(sb.ToString()));
                                    SkipByte(rd);
                                    SkipWrite(wt);
                                }
                                else
                                {
                                    num = rd.ReadInt32();
                                    wt.Write(num);
                                    wt.Write(rd.ReadBytes(num));
                                    SkipByte(rd);
                                    SkipWrite(wt);
                                }
                                
                            }
                            wt.Write(rd.ReadBytes(12));
                        }
                        wt.Write(rd.ReadBytes((int)rd.BaseStream.Length - (int)rd.BaseStream.Position));
                        wt.Close();
                        rd.Close();
                    }
                    else
                    {
                        string[] lines = File.ReadAllLines(opf.FileName);
                        BinaryReader rd = new BinaryReader(new FileStream(Path.GetDirectoryName(opf.FileName) + "\\" + Path.GetFileNameWithoutExtension(opf.FileName) + "_bk", FileMode.Open, FileAccess.Read, FileShare.Read));
                        BinaryWriter wt = new BinaryWriter(new FileStream(Path.GetDirectoryName(opf.FileName) + "\\" + Path.GetFileNameWithoutExtension(opf.FileName), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite));
                        wt.Write(rd.ReadBytes(224));
                        int num = rd.ReadInt32();
                        wt.Write(num);

                        //Actors.xml
                        foreach(XElement element in XElement.Load(lines[0]).Elements("Actor"))
                        {
                            int id = rd.ReadInt32();
                            wt.Write(id);
                            int but = rd.ReadInt32();
                            wt.Write(but);
                            for (int j = 0; j < but; j++)
                            {
                                int n = rd.ReadInt32();
                                wt.Write(n);
                                byte[] h = rd.ReadBytes(n);
                                wt.Write(h);
                                SkipWrite(wt);
                                StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(h));
                                key.Replace(" ", "-");
                                SkipByte(rd);
                                n = rd.ReadInt32();
                                StringBuilder value = new StringBuilder(element.Attribute(key.ToString()).Value);
                                value.Replace("[r]", "\r");
                                value.Replace("[n]", "\n");
                                rd.ReadBytes(n);
                                wt.Write(Encoding.UTF8.GetBytes(value.ToString()).Length);
                                wt.Write(Encoding.UTF8.GetBytes(value.ToString()));
                                SkipWrite(wt);
                                SkipByte(rd);
                                wt.Write(rd.ReadInt32());
                                n = rd.ReadInt32();
                                wt.Write(n);
                                wt.Write(rd.ReadBytes(n));
                                SkipByte(rd);
                                SkipWrite(wt);
                            }
                            wt.Write(rd.ReadBytes(32));
                        }

                        num = rd.ReadInt32();
                        wt.Write(num);
                        //Items.xml
                        foreach (XElement element in XElement.Load(lines[1]).Elements("Item"))
                        {
                            int id = rd.ReadInt32();
                            wt.Write(id);
                            int but = rd.ReadInt32();
                            wt.Write(but);
                            for (int j = 0; j < but; j++)
                            {
                                int n = rd.ReadInt32();
                                wt.Write(n);
                                byte[] h = rd.ReadBytes(n);
                                wt.Write(h);
                                SkipWrite(wt);
                                StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(h));
                                key.Replace(" ", "-");
                                SkipByte(rd);
                                n = rd.ReadInt32();
                                StringBuilder value = new StringBuilder(element.Attribute(key.ToString()).Value);
                                value.Replace("[r]", "\r");
                                value.Replace("[n]", "\n");
                                rd.ReadBytes(n);
                                wt.Write(Encoding.UTF8.GetBytes(value.ToString()).Length);
                                wt.Write(Encoding.UTF8.GetBytes(value.ToString()));
                                SkipWrite(wt);
                                SkipByte(rd);
                                wt.Write(rd.ReadInt32());
                                n = rd.ReadInt32();
                                wt.Write(n);
                                wt.Write(rd.ReadBytes(n));
                                SkipByte(rd);
                                SkipWrite(wt);
                            }
                        }

                        num = rd.ReadInt32();
                        wt.Write(num);
                        //Locations.xml
                        foreach (XElement element in XElement.Load(lines[2]).Elements("Location"))
                        {
                            int id = rd.ReadInt32();
                            wt.Write(id);
                            int but = rd.ReadInt32();
                            wt.Write(but);
                            for (int j = 0; j < but; j++)
                            {
                                int n = rd.ReadInt32();
                                wt.Write(n);
                                byte[] h = rd.ReadBytes(n);
                                wt.Write(h);
                                SkipWrite(wt);
                                StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(h));
                                key.Replace(" ", "-");
                                SkipByte(rd);
                                n = rd.ReadInt32();
                                StringBuilder value = new StringBuilder(element.Attribute(key.ToString()).Value);
                                value.Replace("[r]", "\r");
                                value.Replace("[n]", "\n");
                                rd.ReadBytes(n);
                                wt.Write(Encoding.UTF8.GetBytes(value.ToString()).Length);
                                wt.Write(Encoding.UTF8.GetBytes(value.ToString()));
                                SkipWrite(wt);
                                SkipByte(rd);
                                wt.Write(rd.ReadInt32());
                                n = rd.ReadInt32();
                                wt.Write(n);
                                wt.Write(rd.ReadBytes(n));
                                SkipByte(rd);
                                SkipWrite(wt);
                            }
                        }

                        num = rd.ReadInt32();
                        wt.Write(num);
                        //Variables.xml
                        foreach (XElement element in XElement.Load(lines[3]).Elements("Variable"))
                        {
                            int id = rd.ReadInt32();
                            wt.Write(id);
                            int but = rd.ReadInt32();
                            wt.Write(but);
                            for (int j = 0; j < but; j++)
                            {
                                int n = rd.ReadInt32();
                                wt.Write(n);
                                byte[] h = rd.ReadBytes(n);
                                wt.Write(h);
                                SkipWrite(wt);
                                StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(h));
                                key.Replace(" ", "-");
                                SkipByte(rd);
                                n = rd.ReadInt32();
                                StringBuilder value = new StringBuilder(element.Attribute(key.ToString()).Value);
                                value.Replace("[r]", "\r");
                                value.Replace("[n]", "\n");
                                rd.ReadBytes(n);
                                wt.Write(Encoding.UTF8.GetBytes(value.ToString()).Length);
                                wt.Write(Encoding.UTF8.GetBytes(value.ToString()));
                                SkipWrite(wt);
                                SkipByte(rd);
                                wt.Write(rd.ReadInt32());
                                n = rd.ReadInt32();
                                wt.Write(n);
                                wt.Write(rd.ReadBytes(n));
                                SkipByte(rd);
                                SkipWrite(wt);
                            }
                        }

                        num = rd.ReadInt32();
                        wt.Write(num);
                        //
                        foreach(XElement element in XElement.Load(lines[4]).Elements("Conversation"))
                        {
                            int id = rd.ReadInt32();
                            wt.Write(id);
                            int but = rd.ReadInt32();
                            wt.Write(but);
                            for (int j = 0; j < but; j++)
                            {
                                int n = rd.ReadInt32();
                                wt.Write(n);
                                byte[] h = rd.ReadBytes(n);
                                wt.Write(h);
                                SkipWrite(wt);
                                StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(h));
                                key.Replace(" ", "-");
                                SkipByte(rd);
                                n = rd.ReadInt32();
                                StringBuilder value = new StringBuilder(element.Attribute(key.ToString()).Value);
                                value.Replace("[r]", "\r");
                                value.Replace("[n]", "\n");
                                rd.ReadBytes(n);
                                wt.Write(Encoding.UTF8.GetBytes(value.ToString()).Length);
                                wt.Write(Encoding.UTF8.GetBytes(value.ToString()));
                                SkipWrite(wt);
                                SkipByte(rd);
                                wt.Write(rd.ReadInt32());
                                n = rd.ReadInt32();
                                wt.Write(n);
                                wt.Write(rd.ReadBytes(n));
                                SkipByte(rd);
                                SkipWrite(wt);
                            }
                            wt.Write(rd.ReadBytes(40));
                            but = rd.ReadInt32();
                            wt.Write(but);
                            wt.Write(rd.ReadBytes(but));
                            SkipByte(rd);
                            SkipWrite(wt);
                            but = rd.ReadInt32();
                            wt.Write(but);
                            wt.Write(rd.ReadBytes(but));
                            SkipByte(rd);
                            SkipWrite(wt);
                            but = rd.ReadInt32();
                            wt.Write(but);
                            wt.Write(rd.ReadBytes(but));
                            SkipByte(rd);
                            SkipWrite(wt);
                            wt.Write(rd.ReadBytes(16));
                            but = rd.ReadInt32();
                            wt.Write(but);
                            wt.Write(rd.ReadBytes(but));
                            SkipByte(rd);
                            SkipWrite(wt);
                            but = rd.ReadInt32();
                            wt.Write(but);
                            foreach (XElement elementLow in element.Elements("DialogueEntry"))
                            {
                                id = rd.ReadInt32();
                                wt.Write(id);
                                int kbut = rd.ReadInt32();
                                wt.Write(kbut);
                                for (int k = 0; k < kbut; k++)
                                {
                                    int n = rd.ReadInt32();
                                    wt.Write(n);
                                    byte[] h = rd.ReadBytes(n);
                                    wt.Write(h);
                                    SkipWrite(wt);
                                    StringBuilder key = new StringBuilder(Encoding.UTF8.GetString(h));
                                    key.Replace(" ", "-");
                                    SkipByte(rd);
                                    n = rd.ReadInt32();
                                    StringBuilder value = new StringBuilder(elementLow.Attribute(key.ToString()).Value);
                                    value.Replace("[r]", "\r");
                                    value.Replace("[n]", "\n");
                                    rd.ReadBytes(n);
                                    wt.Write(Encoding.UTF8.GetBytes(value.ToString()).Length);
                                    wt.Write(Encoding.UTF8.GetBytes(value.ToString()));
                                    SkipWrite(wt);
                                    SkipByte(rd);
                                    wt.Write(rd.ReadInt32());
                                    n = rd.ReadInt32();
                                    wt.Write(n);
                                    wt.Write(rd.ReadBytes(n));
                                    SkipByte(rd);
                                    SkipWrite(wt);
                                }
                                wt.Write(rd.ReadBytes(12));
                                kbut = rd.ReadInt32();
                                wt.Write(kbut);
                                wt.Write(rd.ReadBytes(kbut));
                                SkipByte(rd);
                                SkipWrite(wt);
                                wt.Write(rd.ReadInt32());
                                kbut = rd.ReadInt32();
                                wt.Write(kbut);
                                wt.Write(rd.ReadBytes(kbut));
                                SkipByte(rd);
                                SkipWrite(wt);
                                wt.Write(rd.ReadInt32());
                                kbut = rd.ReadInt32();
                                wt.Write(kbut);
                                wt.Write(rd.ReadBytes(kbut * 24));
                                kbut = rd.ReadInt32();
                                wt.Write(kbut);
                                wt.Write(rd.ReadBytes(kbut));
                                SkipByte(rd);
                                SkipWrite(wt);
                                kbut = rd.ReadInt32();
                                wt.Write(kbut);
                                wt.Write(rd.ReadBytes(kbut));
                                SkipByte(rd);
                                SkipWrite(wt);
                                kbut = rd.ReadInt32();
                                wt.Write(kbut);
                                wt.Write(rd.ReadBytes(kbut * 24));
                                wt.Write(rd.ReadBytes(16));
                            }
                            wt.Write(rd.ReadBytes(12));
                        }
                        wt.Write(rd.ReadBytes((int)rd.BaseStream.Length - (int)rd.BaseStream.Position));
                        wt.Close();
                        rd.Close();
                    }
                    MessageBox.Show("Done!!");
                }
            }
        }

        public void SkipByte(BinaryReader rd)
        {
        Continue:;
            if (rd.BaseStream.Position % 4 != 0)
            {
                rd.BaseStream.Seek(1, SeekOrigin.Current);
                goto Continue;
            }
        }

        public void SkipWrite(BinaryWriter wt)
        {
        Writer:;
            if(wt.BaseStream.Position % 4 != 0)
            {
                wt.Write((byte)0x00);
                goto Writer;
            }
        }

        public static void Copy(string inputFilePath, string outputFilePath)
        {
            int bufferSize = 1024 * 1024;
            using (FileStream fileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                FileStream fs = new FileStream(inputFilePath, FileMode.Open, FileAccess.ReadWrite);
                fileStream.SetLength(fs.Length);
                int bytesRead = -1;
                byte[] bytes = new byte[bufferSize];

                while ((bytesRead = fs.Read(bytes, 0, bufferSize)) > 0)
                {
                    fileStream.Write(bytes, 0, bytesRead);
                }
            }
        }
    }
}

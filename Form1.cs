using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace JSONReader
{
    public partial class Form1 : Form
    {
        string file;

        public Form1(string _file)
        {
            InitializeComponent();
            file = _file;
        }

        private string varCheck(string obj)
        {
            string cur = obj;

            cur = cur.Replace("$(folder)", Environment.CurrentDirectory);

            return cur;
        }

        private TreeNode Json2Tree(JObject obj)
        {
            TreeNode parent = new TreeNode();

            foreach (var token in obj)
            {
                parent.Text = varCheck(varCheck(token.Key.ToString()));

                TreeNode child = new TreeNode();
                child.Text = varCheck(varCheck(token.Key.ToString()));

                if (token.Value.Type.ToString() == "Object")
                {
                    JObject o = (JObject)token.Value;
                    child = Json2Tree(o);
                    parent.Nodes.Add(child);
                }
                else if (token.Value.Type.ToString() == "Array")
                {
                    int ix = -1;

                    foreach (var itm in token.Value)
                    {
                        if (itm.Type.ToString() == "Object")
                        {
                            TreeNode objTN = new TreeNode();

                            ix++;

                            JObject o = (JObject)itm;
                            objTN = Json2Tree(o);
                            objTN.Text = varCheck(varCheck(token.Key.ToString())) + "[" + ix + "]";
                            child.Nodes.Add(objTN);
                        }
                        else if (itm.Type.ToString() == "Array")
                        {
                            ix++;
                            TreeNode dataArray = new TreeNode();
                            foreach (var data in itm)
                            {
                                dataArray.Text = varCheck(token.Key.ToString()) + "[" + ix + "]";
                                dataArray.Nodes.Add(data.ToString());
                            }
                            child.Nodes.Add(dataArray);
                        }
                        else
                        {
                            child.Nodes.Add(varCheck(itm.ToString()));
                        }
                    }
                    parent.Nodes.Add(child);
                }
                else
                {
                    if (token.Value.ToString() == "")
                        child.Nodes.Add("N/A");
                    else
                        child.Nodes.Add(varCheck(token.Value.ToString()));
                    parent.Nodes.Add(child);
                }
            }

            return parent;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(file.Trim() == "")
            {
                MessageBox.Show("Please drag your JSON file onto the .exe");
                Environment.Exit(1);
            }

            JObject json = JObject.Parse(File.ReadAllText(file));
            TreeNode nodes = Json2Tree(json);

            for (var i = 0; i < nodes.Nodes.Count; i += 1)
            {
                jsonView.Nodes.Add(nodes.Nodes[i]);
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

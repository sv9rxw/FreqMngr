using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FreqMngr.ViewModels
{
    class FreqGroup
    {
        public static String XML_ATTR_NAME = "name";

        private FreqGroup _Parent { get; set; } = null;

        public String Name
        {
            get
            {
                return XmlNode.Attributes[XML_ATTR_NAME].Value;
            }
            set
            {
                String name = XmlNode.Attributes[XML_ATTR_NAME].Value;
                if (value == name)
                    return;

                XmlNode.Attributes[Freq.XML_ATTR_NAME].Value = value;
            }
        }

        private XmlNode _XmlNode = null;
        public XmlNode XmlNode
        {
            get { return _XmlNode; }
            set { _XmlNode = value; }
        }

        private List<FreqGroup> _ChildGroups = null;
        public List<FreqGroup> ChildGroups
        {
            get { return _ChildGroups; }
            set
            {
                if (value == _ChildGroups)
                    return;

                _ChildGroups = value;
            }
        }
        
        public ObservableCollection<Freq> AllFreqs
        {
            get
            {
                List<Freq> result = new List<Freq>();

                if (this.ChildGroups!=null)
                {
                    foreach (FreqGroup group in this.ChildGroups)
                    {
                        result.AddRange(group.AllFreqs);
                    }       
                }
                if (this.Freqs!=null)
                {
                    foreach(Freq freq in this.Freqs)
                    {
                        result.Add(freq);
                    }                    
                }
                return new ObservableCollection<Freq>(result);
            }
        }

        private List<Freq> _Freqs = null;
        public List<Freq> Freqs
        {
            get
            {
                return _Freqs;
            }
            set
            {
                if (value == _Freqs)
                    return;

                _Freqs = value;
            }
        }

        public void AddNewFreq(Freq freq)
        {
            this.Freqs.Add(freq);
            this.XmlNode.AppendChild(freq.XmlNode);
        }

        public void RemoveFreq(Freq freq)
        {
            this.Freqs.Remove(freq);
            this.XmlNode.RemoveChild(freq.XmlNode);
        }

        public void AddNewGroup(FreqGroup group)
        {
            this.ChildGroups.Add(group);
            this._XmlNode.AppendChild(group.XmlNode);
        }

        public void RemoveGroup(FreqGroup group)
        {
            this.ChildGroups.Remove(group);
            this._XmlNode.RemoveChild(group.XmlNode);
        }
                                  
        public FreqGroup(XmlNode xmlNode)
        {
            if (xmlNode == null) throw new ArgumentException("Linked XML node is null");
            this._XmlNode = xmlNode;

            this._ChildGroups = new List<FreqGroup>();
            this._Freqs = new List<Freq>();
        }

        public FreqGroup(XmlDocument doc, FreqGroup parent, String name)
        {
            if (doc == null) throw new ArgumentException("XML document is null");
            if (parent==null) throw new ArgumentException("Parent group is null");
            if (String.IsNullOrWhiteSpace(name)==true) throw new ArgumentException("Name is null or whitespace");

            this._Parent = parent;

            this._XmlNode = doc.CreateNode(XmlNodeType.Element, "group", parent.Name);
            XmlAttribute attrName = doc.CreateAttribute(XML_ATTR_NAME);
            attrName.Value = name;
            this._XmlNode.Attributes.Append(attrName);

            this._ChildGroups = new List<FreqGroup>();
            this._Freqs = new List<Freq>();
        }


        public override string ToString()
        {
            return this.Name;
        }
    }
}

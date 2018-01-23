using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FreqMngr.ViewModels
{    
    class Freq : INotifyPropertyChanged
    {
        public static String XML_ATTR_NAME = "name";
        public static String XML_ATTR_FREQUENCY = "frequency";
        public static String XML_ATTR_BANDWIDTH = "bandwidth";
        public static String XML_ATTR_MODULATION = "modulation";
        public static String XML_ATTR_MODULATIONTYPE = "modulationtype";
        public static String XML_ATTR_PROTOCOL = "protocol";
        public static String XML_ATTR_COUNTRY = "country";
        public static String XML_ATTR_USER = "user";
        public static String XML_ATTR_COORDINATES = "coordinates";        
        public static String XML_ATTR_QSL = "qsl";

        public static String XML_NODE_DESCRIPTION = "description";
        public static String XML_NODE_URLS = "urls";


        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        private NumberFormatInfo NumberFormat { get; set; } = null;

        private XmlNode _XmlNode = null;
        public XmlNode XmlNode
        {
            get
            {                
                return _XmlNode;
            }
            set
            {
                if (value == _XmlNode)
                    return;

                _XmlNode = value;
            }
        }

        private FreqGroup _Parent = null;        

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

        private double FrequencyInKHz { get; set; }

        public String Frequency
        {
            get
            {
                String freqRawStr = XmlNode.Attributes[XML_ATTR_FREQUENCY].Value;
                double freqDoube = double.Parse(freqRawStr, NumberFormat);
                this.FrequencyInKHz = freqDoube;
                return freqDoube.ToString(this.NumberFormat);
            }
            set
            {
                double freqDouble;
                try
                {
                    freqDouble = double.Parse(value, NumberFormat);
                }
                catch(Exception expt)
                {
                    return;
                }
                
                if (freqDouble == this.FrequencyInKHz)
                    return;

                this.FrequencyInKHz = freqDouble;
                XmlNode.Attributes[Freq.XML_ATTR_FREQUENCY].Value = freqDouble.ToString(NumberFormat);
            }
        }

        private double BandwidthInKHz { get; set; }
        public String Bandwidth
        {
            get
            {
                String bwRawStr;
                try
                {
                    bwRawStr = XmlNode.Attributes[XML_ATTR_BANDWIDTH]?.Value;
                }
                catch(Exception expt)
                {
                    return null;
                }

                if (String.IsNullOrWhiteSpace(bwRawStr) == true)
                    return null;

                double bwDouble = double.Parse(bwRawStr, NumberFormat);
                this.BandwidthInKHz = bwDouble;
                return bwDouble.ToString(NumberFormat);
            }
            set
            {
                double bwDouble;
                try
                {
                    bwDouble = double.Parse(value, NumberFormat);
                }
                catch (Exception expt)
                {
                    return;
                }

                if (bwDouble == this.BandwidthInKHz)
                    return;

                this.BandwidthInKHz = bwDouble;
                try
                {
                    XmlNode.Attributes[Freq.XML_ATTR_BANDWIDTH].Value = bwDouble.ToString(NumberFormat);
                }
                catch(Exception expt)
                {
                    XmlAttribute attr = XmlNode.OwnerDocument.CreateAttribute(XML_ATTR_BANDWIDTH);
                    attr.Value = bwDouble.ToString(NumberFormat); 
                    XmlNode.Attributes.Append(attr);
                }
            }
        }

        
        public String Modulation
        {
            get
            {
                String result = null;
                try
                {
                    result = XmlNode.Attributes[XML_ATTR_MODULATION]?.Value;
                }
                catch(Exception expt)
                {
                    return null;
                }
                return result;                
            }
            set
            {
                try
                {
                    if (value == XmlNode.Attributes[Freq.XML_ATTR_MODULATION].Value)
                        return;

                    XmlNode.Attributes[Freq.XML_ATTR_MODULATION].Value = value;
                }
                catch(Exception expt)
                {
                    XmlAttribute attr = XmlNode.OwnerDocument.CreateAttribute(XML_ATTR_MODULATION);
                    attr.Value = value;
                    XmlNode.Attributes.Append(attr);
                }
            }
        }
        
        public String ModulationType
        {
            get
            {
                String result = null;
                try
                {
                    result = XmlNode.Attributes[XML_ATTR_MODULATIONTYPE]?.Value;
                }
                catch (Exception expt)
                {
                    return null;
                }
                return result;
            }
            set
            {
                try
                {
                    if (value == XmlNode.Attributes[Freq.XML_ATTR_MODULATIONTYPE].Value)
                        return;

                    XmlNode.Attributes[Freq.XML_ATTR_MODULATIONTYPE].Value = value;
                }
                catch (Exception expt)
                {
                    XmlAttribute attr = XmlNode.OwnerDocument.CreateAttribute(XML_ATTR_MODULATIONTYPE);
                    attr.Value = value;
                    XmlNode.Attributes.Append(attr);
                }
            }
        }

        public String Protocol
        {
            get
            {
                String result = null;
                try
                {
                    result = XmlNode.Attributes[XML_ATTR_PROTOCOL]?.Value;
                }
                catch (Exception expt)
                {
                    return null;
                }
                return result;
            }
            set
            {
                try
                {
                    if (value == XmlNode.Attributes[Freq.XML_ATTR_PROTOCOL].Value)
                        return;

                    XmlNode.Attributes[Freq.XML_ATTR_PROTOCOL].Value = value;
                }
                catch (Exception expt)
                {
                    XmlAttribute attr = XmlNode.OwnerDocument.CreateAttribute(XML_ATTR_PROTOCOL);
                    attr.Value = value;
                    XmlNode.Attributes.Append(attr);
                }
            }
        }

        public String Country
        {
            get
            {
                String result = null;
                try
                {
                    result = XmlNode.Attributes[XML_ATTR_COUNTRY]?.Value;
                }
                catch (Exception expt)
                {
                    return null;
                }
                return result;
            }
            set
            {
                try
                {
                    if (value == XmlNode.Attributes[Freq.XML_ATTR_COUNTRY].Value)
                        return;

                    XmlNode.Attributes[Freq.XML_ATTR_COUNTRY].Value = value;
                }
                catch (Exception expt)
                {
                    XmlAttribute attr = XmlNode.OwnerDocument.CreateAttribute(XML_ATTR_COUNTRY);
                    attr.Value = value;
                    XmlNode.Attributes.Append(attr);
                }
            }
        }

        public String User
        {
            get
            {
                String result = null;
                try
                {
                    result = XmlNode.Attributes[XML_ATTR_USER]?.Value;
                }
                catch (Exception expt)
                {
                    return null;
                }
                return result;
            }
            set
            {
                try
                {
                    if (value == XmlNode.Attributes[Freq.XML_ATTR_USER].Value)
                        return;

                    XmlNode.Attributes[Freq.XML_ATTR_USER].Value = value;
                }
                catch (Exception expt)
                {
                    XmlAttribute attr = XmlNode.OwnerDocument.CreateAttribute(XML_ATTR_USER);
                    attr.Value = value;
                    XmlNode.Attributes.Append(attr);
                }
            }
        }
        

        public String Coordinates
        {
            get
            {
                String result = null;                
                try
                {
                    result = XmlNode.Attributes[XML_ATTR_COORDINATES]?.Value;
                }
                catch (Exception expt)
                {
                    return null;
                }             

                return result;
            }
            set
            {
                try
                {
                    if (value == XmlNode.Attributes[Freq.XML_ATTR_COORDINATES].Value)
                        return;

                    XmlNode.Attributes[Freq.XML_ATTR_COORDINATES].Value = value;
                }
                catch (Exception expt)
                {
                    XmlAttribute attr = XmlNode.OwnerDocument.CreateAttribute(XML_ATTR_COORDINATES);
                    attr.Value = value;
                    XmlNode.Attributes.Append(attr);
                }
            }
        }

        public String Description
        {
            get
            {
                String result = null;
                try
                {
                    XmlNodeList childNodes = this._XmlNode.ChildNodes;
                    result = (String)childNodes[0].InnerText;                   
                }
                catch (Exception expt)
                {
                    return null;
                }
                return result;
            }
            set
            {
                XmlNodeList childNodes = this._XmlNode.ChildNodes;

                if (value == (String)childNodes[0].InnerText)
                    return;
                                        
                _XmlNode.ChildNodes[0].InnerText = value;
            }
        }

        public String URLs
        {
            get
            {
                String result = null;
                try
                {
                    XmlNodeList childNodes = this._XmlNode.ChildNodes;
                    result = (String)childNodes[1].InnerText;
                }
                catch (Exception expt)
                {
                    return null;
                }
                return result;
            }
            set
            {
                XmlNodeList childNodes = this._XmlNode.ChildNodes;

                if (value == (String)childNodes[1].InnerText)
                    return;

                _XmlNode.ChildNodes[1].InnerText = value;
            }
        }

        private String QSLStr
        {
            get
            {
                String result = null;
                try
                {
                    result = XmlNode.Attributes[XML_ATTR_QSL]?.Value;
                }
                catch (Exception expt)
                {
                    return null;
                }
                return result;
            }
            set
            {
                if (XmlNode.Attributes[Freq.XML_ATTR_QSL] != null)
                {
                    if (value == XmlNode.Attributes[Freq.XML_ATTR_QSL].Value)
                        return;

                    Debug.WriteLine("aaaa " + value.ToString());
                    XmlNode.Attributes[Freq.XML_ATTR_QSL].Value = value;
                }                    
                else
                {
                    XmlAttribute attr = XmlNode.OwnerDocument.CreateAttribute(XML_ATTR_QSL);
                    attr.Value = value;
                    XmlNode.Attributes.Append(attr);
                }
            }
        }

        public bool QSL
        {
            get
            {
                bool result = false;
                if (QSLStr!=null)
                {                    
                    try
                    {
                        result = bool.Parse(QSLStr);
                    }
                    catch(Exception expt)
                    {
                        return false;
                    }
                    return result;
                }
                return false;
            }
            set
            {
                Debug.WriteLine("QSL: " + value.ToString());
                QSLStr = value.ToString();

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(QSL)));
            }
        }


        public FreqGroup GetParent()
        {
            return this._Parent;
        }

        public void SetParent(FreqGroup group)
        {
            this._Parent = group;
        }

        public Freq(XmlNode xmlNode, FreqGroup parent)
        {
            if (xmlNode == null) throw new ArgumentException("Linked XML node for freq is null");
            if (parent == null) throw new ArgumentException("Parent group is null");
            this._XmlNode = xmlNode;

            this._Parent = parent;

            this.NumberFormat = new NumberFormatInfo();
            this.NumberFormat.NumberDecimalSeparator = ".";
            this.NumberFormat.NumberGroupSeparator = ",";
        }

        public Freq(XmlDocument doc, FreqGroup parent, String name, String frequency, String bandwidth, String modulation, String modulationType, String protocol, String country, String user, String coordinates, String description, String urls)
        {
            if (doc == null) throw new ArgumentException("XML document is null");
            if (parent == null) throw new ArgumentException("Parent group is null");
            if (String.IsNullOrWhiteSpace(name) == true) throw new ArgumentException("Name is null or whitespace");
            if (String.IsNullOrWhiteSpace(frequency) == true) throw new ArgumentException("Frequency is null or whitespace");
            try
            {
                double.Parse(frequency);
            }
            catch(Exception expt)
            {
                throw new ArgumentException("Invalid frequency: " + expt.Message);
            }

            this.NumberFormat = new NumberFormatInfo();
            this.NumberFormat.NumberDecimalSeparator = ".";
            this.NumberFormat.NumberGroupSeparator = ",";

            this._XmlNode = doc.CreateNode(XmlNodeType.Element, "freq", parent.Name);
            AddXMLAttribute(doc, this._XmlNode, XML_ATTR_NAME, name);
            AddXMLAttribute(doc, this._XmlNode, XML_ATTR_FREQUENCY, frequency);                        
            AddXMLAttribute(doc, this._XmlNode, XML_ATTR_BANDWIDTH, bandwidth);
            AddXMLAttribute(doc, this._XmlNode, XML_ATTR_MODULATION, modulation);
            AddXMLAttribute(doc, this._XmlNode, XML_ATTR_MODULATIONTYPE, modulationType);
            AddXMLAttribute(doc, this._XmlNode, XML_ATTR_PROTOCOL, protocol);
            AddXMLAttribute(doc, this._XmlNode, XML_ATTR_COUNTRY, country);
            AddXMLAttribute(doc, this._XmlNode, XML_ATTR_USER, user);
            AddXMLAttribute(doc, this._XmlNode, XML_ATTR_COORDINATES, coordinates);

            XmlNode descriptionNode = doc.CreateNode(XmlNodeType.Element, XML_NODE_DESCRIPTION, parent.Name);
            descriptionNode.InnerText = description;
            this._XmlNode.AppendChild(descriptionNode);

            XmlNode urlsNode = doc.CreateNode(XmlNodeType.Element, XML_NODE_URLS, parent.Name);
            urlsNode.InnerText = urls;
            this._XmlNode.AppendChild(urlsNode);

            this._Parent = parent;
        }

        private void AddXMLAttribute(XmlDocument doc, XmlNode node, String attribute_name, String attribute_value)
        {
            XmlAttribute attr = doc.CreateAttribute(attribute_name);
            if (attribute_value == null)
                attr.Value = String.Empty;
            else
                attr.Value = attribute_value;
            node.Attributes.Append(attr);
        }

        public Freq Clone()
        {
            Freq freq = null;
            XmlNode node = this.XmlNode.Clone();
            freq = new Freq(node, this._Parent);
            return freq;
        }
    }
}

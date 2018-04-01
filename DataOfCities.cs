using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Cities
{
    class City
    {
        private List<ListCities> _Cities;
        private string _FileName, _ErrorString;
        private int _Count;
        private Exception _Error;

        #region Конструктор

        /// <summary>
        /// Конструктор класса DataSet
        /// </summary>
        public City()
        {
            _FileName = "";
            _ErrorString = "";
            _Cities = new List<ListCities>();
            _Count = 0;
        }

        #endregion

        #region Свойства

        /// <summary>
        /// Свойство, которое возвращает список городов
        /// </summary>
        public List<ListCities> GetListCities
        {
            get { return _Cities; }
        }

        /// <summary>
        /// Свойство, которое возвращает количество записей
        /// </summary>
        public int GetCount
        {
            get { return _Count; }
        }

        #endregion

        #region Метод

        /// <summary>
        /// Метод, отвечающий за загрузку данных из файла с расширением ".xml"
        /// </summary>
        /// <param name="FileName">Полное имя файла, с которым осуществляется работа</param>
        public void OutXML(string FileName)
        {
            _FileName = FileName;
            try
            {
                _FileName = System.IO.Path.ChangeExtension(_FileName, "xml");
                
                using (XmlTextReader reader = new XmlTextReader(_FileName))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement("City"))
                        {
                            _Cities.Add
                            (
                            new ListCities
                            {
                                Id = Convert.ToInt32(reader.GetAttribute("id")),
                                Name = Convert.ToString(reader.GetAttribute("Name")),
                                CountryCode = Convert.ToString(reader.GetAttribute("CountryCode")),
                                District = Convert.ToString(reader.GetAttribute("District")),
                                Population = Convert.ToInt32(reader.GetAttribute("Population"))
                            });
                        }
                        else if (!reader.IsStartElement() && reader.Name == "configuration") break;
                    }
                }
                _Count = _Cities.Count;
                _ErrorString = "";
                _Error = null;
            }
            catch (ArgumentNullException e)
            {
                _ErrorString = "Файл не выбран";
                _Error = e;
            }
            catch (Exception e)
            {
                _ErrorString = "" + e;
                _Error = e;
            }
        }

        #endregion
    }

    class ListCities
    {
        private int _Id, _Population;
        private string _Name, _CountryCode, _District;

        #region Свойства

        /// <summary>
        /// Свойство, которое возвращает и получает значение для id
        /// </summary>
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        /// <summary>
        /// Свойство, которое возвращает и получает значение для Name
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// Свойство, которое возвращает и получает значение для Country Code
        /// </summary>
        public string CountryCode
        {
            get { return _CountryCode; }
            set { _CountryCode = value; }
        }

        /// <summary>
        /// Свойство, которое возвращает и получает значение для District
        /// </summary>
        public string District
        {
            get { return _District; }
            set { _District = value; }
        }

        /// <summary>
        /// Свойство, которое возвращает и получает значение для Population
        /// </summary>
        public int Population
        {
            get { return _Population; }
            set { _Population = value; }
        }

        #endregion
    }
}

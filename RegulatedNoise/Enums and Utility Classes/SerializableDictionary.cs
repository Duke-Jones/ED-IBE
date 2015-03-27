using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RegulatedNoise.Enums_and_Utility_Classes
{
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region Constructors

        public SerializableDictionary() : base() { }

        public SerializableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

        public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }

        public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }

        public SerializableDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }

        public SerializableDictionary(int capacity) : base(capacity) { }

        #endregion

        private const string ItemTagName = "item";
        private const string KeyTagName = "key";
        private const string ValueTagName = "value";

        /// <summary>
        /// Diese Methode ist reserviert und sollte nicht verwendet werden. Wenn Sie die IXmlSerializable-Schnittstelle implementieren, sollten Sie null (Nothing in Visual Basic) von der Methode zurückgeben und stattdessen das <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> auf die Klasse anwenden, wenn ein benutzerdefiniertes Schema erforderlich ist.
        /// </summary>
        /// <returns>
        /// Ein <see cref="T:System.Xml.Schema.XmlSchema"/> zur Beschreibung der XML-Darstellung des Objekts, das von der <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/>-Methode erstellt und von der <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/>-Methode verwendet wird.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generiert ein Objekt aus seiner XML-Darstellung.
        /// </summary>
        /// <param name="reader">Der <see cref="T:System.Xml.XmlReader"/>-Stream, aus dem das Objekt deserialisiert wird.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
            {
                return;
            }

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement(ItemTagName);

                reader.ReadStartElement(KeyTagName);
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement(ValueTagName);
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// Konvertiert ein Objekt in seine XML-Darstellung.
        /// </summary>
        /// <param name="writer">Der <see cref="T:System.Xml.XmlWriter"/>-Stream, in den das Objekt serialisiert wird.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement(ItemTagName);

                writer.WriteStartElement(KeyTagName);
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement(ValueTagName);
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
    }
}

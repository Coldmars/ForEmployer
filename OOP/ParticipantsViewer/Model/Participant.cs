using System;

namespace ParticipantsViewer.Model
{
    [Serializable()]
    public class Participant
    {
        [System.Xml.Serialization.XmlElement("Name")]
        public string FirstName { get; set; }

        [System.Xml.Serialization.XmlElement("Surname")]
        public string LastName { get; set; }

        [System.Xml.Serialization.XmlElement("RegisterDate")]
        public DateTime RegistrationDate { get; set; }

        public string ProviderName { get; set;  }

        public override bool Equals(object obj)
        {
            return this.FirstName == (obj as Participant).FirstName &&
                   this.LastName == (obj as Participant).LastName &&
                   this.RegistrationDate == (obj as Participant).RegistrationDate;
        }

        public override int GetHashCode() =>
            (FirstName + LastName + RegistrationDate).GetHashCode();

    }
}

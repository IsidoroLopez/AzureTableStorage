using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTableStorage.Models
{
    public class Animales : TableEntity 
    {
        public String Nombre { get; set; }
        public int Edad { get; set; }
        private String _IdAnimal;
        public string IdAnimal
        {
            get
            {
                return this._IdAnimal;
            }
            set
            {
                this.RowKey = value;
                _IdAnimal = value;
            }
        }

        private String _Raza;
        public String Raza
        {
            get
            {
                return this._Raza;
            }
            set {
                this.PartitionKey = value;
                this._Raza = value;
            }
        }

    }
}
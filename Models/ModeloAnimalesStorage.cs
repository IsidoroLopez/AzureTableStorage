using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTableStorage.Models
{
    public class ModeloAnimalesStorage
    {


            //DECLARAMOS UN OBJETO STORAGE
            CloudStorageAccount storageAccount;

            public ModeloAnimalesStorage()
            {
                //CREAMOS EL OBJETO STORAGE A PARTIR DE LA CADENA DE CONEXION
                storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            }


            public CloudTable CrearTablaAzureStorage()
            {
                CloudTableClient tableAnimal = storageAccount.CreateCloudTableClient();
                CloudTable tabla = tableAnimal.GetTableReference("mitablaanimales");
                tabla.CreateIfNotExists();
                return tabla;
            }
            //METODO PARA INSERTAR UN ANIMAL EN LA TABLA
            public void GuardarAnimalTabla(String idanimal, String nombre, int edad, String raza)
            {
                CloudTable tabla = this.CrearTablaAzureStorage();
                Animales animal = new Animales();
                animal.Raza = raza;
                animal.IdAnimal = idanimal;
                animal.Nombre = nombre;
                animal.Edad = edad;
                TableOperation insertOperation = TableOperation.Insert(animal);
                tabla.Execute(insertOperation);
            }

            //METODO PARA RECUPERAR UN ANIMAL POR SU ROWKEY
            public Animales GetAnimalTabla(String partitionkey, String rowkey)
            {
                CloudTable tabla = this.CrearTablaAzureStorage();
                TableQuery<Animales> consulta = new TableQuery<Animales>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionkey));
                List<Animales> listaanimales = tabla.ExecuteQuery(consulta).ToList();
                TableRequestOptions request = new TableRequestOptions();
                OperationContext operation = new OperationContext();       
                TableOperation retrieveOperation = TableOperation.Retrieve<Animales>(partitionkey, rowkey);
                TableResult query = tabla.Execute(retrieveOperation);
                if (query.Result != null)
                {
                    Animales c = (Animales)query.Result;
                    return c;
                }
                else
                {
                    return null;
                }
            }

            //METODO PARA RECUPERAR LOS ELEMENTOS DE UNA TABLA A TRAVES DE SU PARTITION KEY
            public List<Animales> GetAnimalesRaza(String partitionkey)
            {
                CloudTable tabla = this.CrearTablaAzureStorage();

                TableQuery<Animales> consulta = new TableQuery<Animales>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionkey));    
                List<Animales> listaanimales = tabla.ExecuteQuery(consulta).ToList();
                if (listaanimales.Count != 0)
                {
                    return listaanimales;
                }
                else
                {
                    return null;
                }
            }


            //METODO PARA MODIFICAR UNA ENTIDAD DE LA TABLA
            public Animales ModificarAnimal(String partitionkey, String rowkey, String nombre, int edad)
            {
                CloudTable tabla = CrearTablaAzureStorage();
                Animales animal = GetAnimalTabla(partitionkey, rowkey);
                if (animal != null)
                {

                    animal.Nombre = nombre;
                    animal.Edad = edad;
                    animal.PartitionKey = partitionkey;
                    TableOperation update = TableOperation.Replace(animal);
                    tabla.Execute(update);
                    return animal;
                }
                else
                {
                    return null;
                }
            }


            //METODO PARA ELIMINAR UN ANIMAL
            public void EliminarAnimal (String partitionkey, String rowkey)
            {
                CloudTable tabla = CrearTablaAzureStorage();
                Animales animal = GetAnimalTabla(partitionkey, rowkey);
                if (animal != null)
                {
                    TableOperation delete = TableOperation.Delete(animal);
                    tabla.Execute(delete);
                }
            }                       
    }
}

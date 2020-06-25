using System;
using System.IO;
using System.Collections.Generic;

namespace Dynatrace
{
    class Program{
        static int Main(string[] args){
          string path_entrada=" ";
          if(args.Length==0){
            Console.WriteLine("Ingrese la ruta del archivo CSV.");
            return 0;
          }else{
            path_entrada=args[0];
            Console.WriteLine("Ruta del archivo CSV: "+path_entrada);
          }

          if(File.Exists(path_entrada)){
          List<string> listA = new List<string>();

          int ncol=0,nrow=0;
          
          
          /**Lee el CSV**/
            using(var reader = new StreamReader(@path_entrada)){        
                   while (!reader.EndOfStream){
                          var line = reader.ReadLine();
                          if(!line.Equals("")){
                          var values = line.Split('\n');
                          listA.Add(values[0]);}
                    }          
            }
          /*****/
          /**Cuenta las  celda, para tener ncol y nrow**/
          int j;
             for (j = 0; j< listA[0].Length; j++){
                    
                     if(listA[0][j]==';' | j==(listA[0].Length-1)){
                          ncol++;
                     }
                }
            nrow=listA.Count;//Numero de lineas
            
          /******/
            string[] Columnas= new string[ncol];

          /*****Obtiene el nombre de las columnas*****/
            string temp="";
                int k=0;
                temp="";

                for (j = 0; j< listA[0].Length; j++){
                    
                     if(listA[0][j]==';' | j==(listA[0].Length-1)){
                          if(j==(listA[0].Length-1)) temp+=listA[0][j];
                          Columnas[k]=temp;
                          k++;
                          temp="";
                     }else{
                         temp+=listA[0][j];
                     }
                }
            listA.RemoveAt(0);//Quita el nombre de las columnas
            nrow--;
            /************/

            /***********/
            string[,] datos= new string[nrow,ncol];

            int i=0;

            foreach (var renglon in listA){   
              j=0;             
                for (int p = 0; p< renglon.Length; p++){                    
                     if(renglon[p]==';' | p==(renglon.Length-1)){
                        if(p==(renglon.Length-1)) temp+=renglon[p];
                          datos[i,j]=temp.Replace(",",".");
                          temp="";
                          j++;
                     }else{
                         temp+=renglon[p];
                     }
                }
                i++;
            }

            /*for(j=0;j<ncol;j++){
            for(i=0;i<nrow;i++){
                  Console.WriteLine(datos[i,j]+" ");
                }
                Console.WriteLine();
            }*/
            /************/
            Console.WriteLine(ncol+" Columnas"+" y "+nrow+" renglones");
            /*foreach(var nomcol in Columnas){
              Console.WriteLine(nomcol);
            }*/

            /***Transformar***/
            string[,] csvtransformado = new string[(ncol-1)*nrow,3];
            k=0;
            /*csvtransformado[0,0]="\"DATE\"";
            csvtransformado[0,1]="\"SERVICE\"";
            csvtransformado[0,2]="\"TIME\"";
            k++;*/
            for(j=1;j<ncol;j++){
              for(i=0;i<nrow;i++){
                  csvtransformado[k,0]=datos[i, 0];
                  csvtransformado[k,1]=Columnas[j];
                  csvtransformado[k,2]=datos[i, j];
                  k++;
              }
            }

           /*******Guardar un archivo a CSV********/
           string path_salida=path_entrada.Substring(0,path_entrada.Length-4)+"_TRANSFORMADO.csv";
          Console.WriteLine("CSV de salida: "+path_salida);
           using (StreamWriter outfile = new StreamWriter(@path_salida)){
                for (int x = 0; x < (ncol-1)*nrow; x++){
                    string content = "";
                  for (int y = 0; y < 3; y++){
                       content += csvtransformado[x, y];
                       if(y<(ncol-1)){
                         content +=";";
                       }
                  }
                  if(x<(ncol-1)*nrow) outfile.WriteLine(content);
            }
           }
          }else{
              Console.WriteLine("El Archivo no existe");
          }
           /**************/
           return 1;
        }
    }
}

﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Sistem_de_inventario
{
    public class Administrador
    {
        //Ejecutar formularios
        public void EjecutarFormCuerinas() { fCuerina formulario = new fCuerina(); formulario.ShowDialog(); }
        public void EjecutarFormPoliester() { FPoliester formulario = new FPoliester(); formulario.ShowDialog(); }
        public void EjecutarFormArticulos() { fArticulos formulario = new fArticulos(); formulario.ShowDialog(); }
        public void EjecutarFormEntradaSimple() { FEntradaSimple formulario = new FEntradaSimple(); formulario.ShowDialog(); }
        //Conexión a base de datos
        private IDbConnection CrearConexionTapiceria()
        {
            try
            {
                //Conexion MySQL
                MySqlConnection conexionTapiceria = new MySqlConnection("datasource=127.0.0.1;port=3306;username=root;password=;database=tapiceriadb;");
                conexionTapiceria.Open();
                return conexionTapiceria;
            }
            catch
            {
                MessageBox.Show("Error al conectar con la base de datos, contacte con el administrador", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(Environment.ExitCode);
                return null; //codigo inaccesible
            }
        }
        private void CrearParametro<T>(IDbCommand auxCommand, string auxNombre, T auxValue)
        {
            IDbDataParameter parametro = auxCommand.CreateParameter();
            parametro.ParameterName = auxNombre;
            parametro.Value = auxValue;
            auxCommand.Parameters.Add(parametro);
        }
        public List<Cuerinas> CargarListaCuerinas(string auxCol, string auxOrd)
        {
            List<Cuerinas> listaCuerinas = new List<Cuerinas>();
            using (IDbConnection conexionIDB = CrearConexionTapiceria())
            {
                using (IDbCommand commandSQL = conexionIDB.CreateCommand())
                {
                    commandSQL.CommandText = $"SELECT * FROM Cuerinas ORDER BY {auxCol} {auxOrd} ";
                    //cuerinasID, Nombre, Ancho, Precio, Porcentaje
                    using (IDataReader dataReader = commandSQL.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            listaCuerinas.Add(new Cuerinas()
                            {
                                ID = Convert.ToInt32(dataReader["cuerinasID"]),
                                Nombre = Convert.ToString(dataReader["Nombre"]),
                                Ancho = Convert.ToDecimal(dataReader["Ancho"]),
                                Precio = Convert.ToDecimal(dataReader["Precio"]),
                                PorcDeGanancia = Convert.ToInt32(dataReader["Porcentaje"]),
                            });
                        }
                    }
                }
            }
            return listaCuerinas;
        }
        public List<Poliester> CargarListaPoliester(string auxCol, string auxOrd)
        {
            List<Poliester> listaPoliester = new List<Poliester>();
            using (IDbConnection conexionIDB = CrearConexionTapiceria())
            {
                using (IDbCommand commandSQL = conexionIDB.CreateCommand())
                {
                    commandSQL.CommandText = $"SELECT * FROM Poliester ORDER BY {auxCol} {auxOrd} ";
                    //poliesterID, Nombre, Precio, Porcentaje
                    using (IDataReader dataReader = commandSQL.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            listaPoliester.Add(new Poliester()
                            {
                                ID = Convert.ToInt32(dataReader["poliesterID"]),
                                Nombre = Convert.ToString(dataReader["Nombre"]),
                                Precio = Convert.ToDecimal(dataReader["Precio"]),
                                PorcDeGananciaPlancha = Convert.ToInt32(dataReader["PorcentajePlancha"]),
                                PorcDeGanancia = Convert.ToInt32(dataReader["Porcentaje"])
                            });
                        }
                    }
                }
            }
            return listaPoliester;
        }
        public List<Articulos> CargarListaArticulos(string auxCol, string auxOrd)
        {
            List<Articulos> listaArticulos = new List<Articulos>();
            using (IDbConnection conexionIDB = CrearConexionTapiceria())
            {
                using (IDbCommand commandSQL = conexionIDB.CreateCommand())
                {
                    commandSQL.CommandText = $"SELECT * FROM Articulos ORDER BY {auxCol} {auxOrd} ";
                    //articulosID, Descripcion, Precio, Porcentaje
                    using (IDataReader dataReader = commandSQL.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            listaArticulos.Add(new Articulos()
                            {
                                ID = Convert.ToInt32(dataReader["articulosID"]),
                                Nombre = Convert.ToString(dataReader["Descripcion"]),
                                Precio = Convert.ToDecimal(dataReader["Precio"]),
                                PorcDeGanancia = Convert.ToInt32(dataReader["Porcentaje"]),
                            });
                        }
                    }
                }
            }
            return listaArticulos;
        }
        public void GuardarNuevoPoliester(Poliester poliester)
        {
            using (IDbConnection conexionIDB = CrearConexionTapiceria())
            {
                using (IDbCommand command = conexionIDB.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Poliester (Nombre, Precio, PorcentajePlancha, Porcentaje) VALUES (@Nombre, @Precio, @PorcPlancha, @PorcDeGanancia)";

                    CrearParametro(command, "Nombre", poliester.Nombre);
                    CrearParametro(command, "Precio", poliester.Precio);
                    CrearParametro(command, "PorcPlancha", poliester.PorcDeGananciaPlancha);
                    CrearParametro(command, "PorcDeGanancia", poliester.PorcDeGanancia);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void GuardarNuevaCuerina(Cuerinas cuerina)
        {
            using (IDbConnection conexionIDB = CrearConexionTapiceria())
            {
                using (IDbCommand command = conexionIDB.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Cuerinas (Nombre, Ancho, Precio, Porcentaje) VALUES (@Nombre, @Ancho, @Precio, @PorcDeGanancia)";

                    CrearParametro(command, "Nombre", cuerina.Nombre);
                    CrearParametro(command, "Ancho", cuerina.Ancho);
                    CrearParametro(command, "Precio", cuerina.Precio);
                    CrearParametro(command, "PorcDeGanancia", cuerina.PorcDeGanancia);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void GuardarNuevoArticulo(Articulos articulo)
        {
            using (IDbConnection conexionIDB = CrearConexionTapiceria())
            {
                using (IDbCommand command = conexionIDB.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Articulos (Descripcion, Precio, Porcentaje) VALUES (@Nombre, @Precio, @PorcDeGanancia)";

                    CrearParametro(command, "Nombre", articulo.Nombre);
                    CrearParametro(command, "Precio", articulo.Precio);
                    CrearParametro(command, "PorcDeGanancia", articulo.PorcDeGanancia);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void ActualizarPoliester(Poliester poliester)
        {
            using (IDbConnection conexionIDB = CrearConexionTapiceria())
            {
                using (IDbCommand command = conexionIDB.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE Poliester SET Nombre=@Nombre, Precio=@Precio, PorcentajePlancha=@PorcPlancha, Porcentaje=@PorcDeGanancia WHERE poliesterID=@ID";

                    CrearParametro(command, "ID", poliester.ID);
                    CrearParametro(command, "Nombre", poliester.Nombre);
                    CrearParametro(command, "Precio", poliester.Precio);
                    CrearParametro(command, "PorcPlancha", poliester.PorcDeGananciaPlancha);
                    CrearParametro(command, "PorcDeGanancia", poliester.PorcDeGanancia);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void ActualizarCuerina(Cuerinas cuerina)
        {
            using (IDbConnection conexionIDB = CrearConexionTapiceria())
            {
                using (IDbCommand command = conexionIDB.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE Cuerinas SET Nombre=@Nombre, Ancho=@Ancho, Precio=@Precio, Porcentaje=@PorcDeGanancia WHERE cuerinasID=@ID";

                    CrearParametro(command, "ID", cuerina.ID);
                    CrearParametro(command, "Nombre", cuerina.Nombre);
                    CrearParametro(command, "Ancho", cuerina.Ancho);
                    CrearParametro(command, "Precio", cuerina.Precio);
                    CrearParametro(command, "PorcDeGanancia", cuerina.PorcDeGanancia);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void ActualizarArticulo(Articulos articulo)
        {
            using (IDbConnection conexionIDB = CrearConexionTapiceria())
            {
                using (IDbCommand command = conexionIDB.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE Articulos SET Descripcion=@Nombre, Precio=@Precio, Porcentaje=@PorcDeGanancia WHERE articulosID=@ID";

                    CrearParametro(command, "ID", articulo.ID);
                    CrearParametro(command, "Nombre", articulo.Nombre);
                    CrearParametro(command, "Precio", articulo.Precio);
                    CrearParametro(command, "PorcDeGanancia", articulo.PorcDeGanancia);

                    command.ExecuteNonQuery();
                }
            }
        }
        public void EliminarItem(int auxID)
        {
            using (IDbConnection conexionIDB = CrearConexionTapiceria())
            {
                using (IDbCommand command = conexionIDB.CreateCommand())
                {
                    switch (FPrincipal.PestañaActual)
                    {
                        case 1:
                            {
                                command.CommandText =
                                "DELETE FROM Poliester WHERE poliesterID=@ID";
                                CrearParametro(command, "ID", auxID);
                                command.ExecuteNonQuery();
                            }; break;
                        case 2:
                            {
                                command.CommandText =
                                "DELETE FROM Cuerinas WHERE cuerinasID=@ID";
                                CrearParametro(command, "ID", auxID);
                                command.ExecuteNonQuery();
                            }; break;
                        case 3:
                            {
                                command.CommandText =
                                "DELETE FROM Articulos WHERE articulosID=@ID";
                                CrearParametro(command, "ID", auxID);
                                command.ExecuteNonQuery();
                            }; break;
                    }
                }
            }
        }
        public void ActualizarPorcentajeArticulos(int auxPorcentaje)
        {
            using (IDbConnection conexionIDB = CrearConexionTapiceria())
            {
                using (IDbCommand command = conexionIDB.CreateCommand())
                {
                    command.CommandText = "UPDATE Articulos SET Porcentaje= Porcentaje + @PorcDeGanancia";

                    CrearParametro(command, "PorcDeGanancia", auxPorcentaje);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

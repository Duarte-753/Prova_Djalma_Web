using MySql.Data.MySqlClient;
using Prova_web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prova_web.Controllers
{
    public class ClienteController : Controller
    {
        //Index e Pesquisar
        public ActionResult Index(Cliente cli)
        {
            var lstVendedores = new List<Usuario>();
            using (var conexao = new Conexao())
            {
                string strVendedores = "SELECT * FROM usuarios where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strVendedores, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var usuario = new Usuario
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"])
                            };

                            lstVendedores.Add(usuario);
                        }
                    ViewBag.ListaVendedores = lstVendedores;
                }
            }

            using (var conexao = new Conexao())
            {

                string strClientes = "SELECT * FROM clientes " +
                "WHERE Nome like @nome and " +
                "isExcluido = false;";

                using (var comando = new MySqlCommand(strClientes, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", cli.Nome + "%");

                    MySqlDataReader dr = comando.ExecuteReader();

                    if (dr.HasRows)
                    {
                        var lstClientes = new List<Cliente>();

                        while (dr.Read())
                        {
                            var cliente = new Cliente
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Telefone = Convert.ToString(dr["telefone"]),
                                EMail = Convert.ToString(dr["email"]),
                                Vendedor = Convert.ToString(dr["vendedor"]),
                                // Para levar pra view, traz do banco de dados
                                // em formato DateTime e converte
                                // para string para formatar para o usuário
                                DataNasc = Convert.ToDateTime(dr["dataNasc"]).ToString("dd/MM/yyyy")
                            };

                            lstClientes.Add(cliente);
                        }
                        ViewBag.ListaClientes = lstClientes;
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }



        // Editar
        public ActionResult EditarCliente(int Id)
        {
            var lstVendedores = new List<Usuario>();
            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM clientes " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var cliente = new Cliente
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = Convert.ToString(dr["nome"]),
                            Telefone = Convert.ToString(dr["telefone"]),
                            EMail = Convert.ToString(dr["email"]),
                            Vendedor = Convert.ToString(dr["vendedor"]),
                            DataNasc = Convert.ToString(dr["dataNasc"])

                        };
                        using (var conexao2 = new Conexao())
                        {
                            string strVendedores = "SELECT * FROM usuarios where isExcluido = false order by nome;";
                            using (var comando2 = new MySqlCommand(strVendedores, conexao2.conn))
                            {
                                MySqlDataReader dr2 = comando2.ExecuteReader();
                                if (dr2.HasRows)
                                    while (dr2.Read())
                                    {
                                        var usuario = new Usuario
                                        {
                                            Id = Convert.ToInt32(dr2["Id"]),
                                            Nome = Convert.ToString(dr2["nome"])
                                        };

                                        lstVendedores.Add(usuario);
                                    }
                                ViewBag.ListaVendedores = lstVendedores;
                            }
                        }
                        return View(cliente);
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Index");
                    }
                }
            }

        }

        [HttpPost]
        public ActionResult SalvarAlteracoesCliente(Cliente cliente)
        {

            using (var conexao = new Conexao())
            {
                string strLogin = "UPDATE clientes SET " +
                                    "Nome = @nome, " +
                                    "EMail = @email, " +
                                    "DataNasc = @dataNasc, " +
                                    "Telefone = @telefone, " +
                                    "Vendedor = @vendedor " +
                                    "where id = @Id;";


                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", cliente.Nome);
                    comando.Parameters.AddWithValue("@email", cliente.EMail);
                    //comando.Parameters.AddWithValue("@dataNasc", cliente.DataNasc);
                    comando.Parameters.AddWithValue("@telefone", cliente.Telefone);
                    comando.Parameters.AddWithValue("@vendedor", cliente.Vendedor);


                    if (DateTime.TryParseExact(cliente.DataNasc, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataNascConvertida))
                    {
                        string dataNascParaBanco = dataNascConvertida.ToString("yyyy/MM/dd HH:mm:ss");
                        comando.Parameters.AddWithValue("@dataNasc", dataNascParaBanco);
                    }
                    else
                    {

                    }

                    comando.Parameters.AddWithValue("@id", cliente.Id);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }
        }

        // Salvar
        public ActionResult NovoCliente()
        {
            var lstVendedores = new List<Usuario>();
            using (var conexao = new Conexao())
            {
                string strVendedores = "SELECT * FROM usuarios where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strVendedores, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var usuario = new Usuario
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"])
                            };

                            lstVendedores.Add(usuario);
                        }
                    ViewBag.ListaVendedores = lstVendedores;
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult SalvarCliente(Cliente cliente)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "INSERT INTO clientes (nome, email, dataNasc, telefone, vendedor ) " +
                                  "values (" +
                                  "@nome, @email, @dataNasc, @telefone, @vendedor);";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", cliente.Nome);
                    comando.Parameters.AddWithValue("@email", cliente.EMail);
                    //comando.Parameters.AddWithValue("@dataNasc", cliente.DataNasc); "não estou usando mais"
                    comando.Parameters.AddWithValue("@telefone", cliente.Telefone);
                    comando.Parameters.AddWithValue("@vendedor", cliente.Vendedor);


                    // Converte a data de nascimento para o formato yyyy/MM/dd
                    if (DateTime.TryParseExact(cliente.DataNasc, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataNascConvertida))
                    {
                        string dataNascParaBanco = dataNascConvertida.ToString("yyyy/MM/dd");
                        comando.Parameters.AddWithValue("@dataNasc", dataNascParaBanco);
                    }
                    else
                    {
                        return RedirectToAction("NovoCliente");
                    }


                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }
        }

        // Visualizar

        public ActionResult VisualizarCliente(int Id)
        {
            var lstVendedores = new List<Usuario>();
            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM clientes " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var cliente = new Cliente
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = Convert.ToString(dr["nome"]),
                            Telefone = Convert.ToString(dr["telefone"]),
                            EMail = Convert.ToString(dr["email"]),
                            Vendedor = Convert.ToString(dr["vendedor"]),
                            DataNasc = Convert.ToString(dr["dataNasc"])

                        };
                        using (var conexao4 = new Conexao())
                        {
                            string strVendedores = "SELECT * FROM usuarios where isExcluido = false order by nome;";
                            using (var comando4 = new MySqlCommand(strVendedores, conexao4.conn))
                            {
                                MySqlDataReader dr2 = comando4.ExecuteReader();
                                if (dr2.HasRows)
                                    while (dr2.Read())
                                    {
                                        var usuario = new Usuario
                                        {
                                            Id = Convert.ToInt32(dr2["Id"]),
                                            Nome = Convert.ToString(dr2["nome"])
                                        };

                                        lstVendedores.Add(usuario);
                                    }
                                ViewBag.ListaVendedores = lstVendedores;
                            }
                        }
                        return View(cliente);
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Index");
                    }
                }
            }
        }

        // Delet parcial

        public ActionResult ExcluirCliente(int Id)
        {
            var lstVendedores = new List<Usuario>();
            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM clientes " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var cliente = new Cliente
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = Convert.ToString(dr["nome"]),
                            Telefone = Convert.ToString(dr["telefone"]),
                            EMail = Convert.ToString(dr["email"]),
                            Vendedor = Convert.ToString(dr["vendedor"]),
                            DataNasc = Convert.ToString(dr["dataNasc"])

                        };
                        using (var conexao2 = new Conexao())
                        {
                            string strVendedores = "SELECT * FROM usuarios where isExcluido = false order by nome;";
                            using (var comando2 = new MySqlCommand(strVendedores, conexao2.conn))
                            {
                                MySqlDataReader dr2 = comando2.ExecuteReader();
                                if (dr2.HasRows)
                                    while (dr2.Read())
                                    {
                                        var usuario = new Usuario
                                        {
                                            Id = Convert.ToInt32(dr2["Id"]),
                                            Nome = Convert.ToString(dr2["nome"])
                                        };

                                        lstVendedores.Add(usuario);
                                    }
                                ViewBag.ListaVendedores = lstVendedores;
                            }
                        }
                        return View(cliente);
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Index");
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult ExcluirSoft(Cliente cliente)
        {
            // DELETE DE FATO

            //using (var conexao = new Conexao())
            //{
            //    string strLogin = "DELETE FROM usuarios " +
            //                        "where id = @Id;";

            //    using (var comando = new MySqlCommand(strLogin, conexao.conn))
            //    {
            //        comando.Parameters.AddWithValue("@id", usuario.Id);
            //        comando.ExecuteNonQuery();

            //        return RedirectToAction("Index");
            //    }
            //}

            // SOFT DELETE


            using (var conexao = new Conexao())
            {
                string strLogin = "UPDATE clientes SET isExcluido = true " +
                                    "where id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@id", cliente.Id);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }

        }

    }
}
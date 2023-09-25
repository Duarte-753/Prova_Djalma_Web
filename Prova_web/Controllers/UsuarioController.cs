using MySql.Data.MySqlClient;
using Prova_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prova_web.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM usuarios WHERE isExcluido = false;";
                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                    {
                        var lstUsuarios = new List<Usuario>();

                        while (dr.Read())
                        {
                            var usuario = new Usuario
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Celular = Convert.ToString(dr["celular"]),
                                UserName = Convert.ToString(dr["userName"])
                            };

                            lstUsuarios.Add(usuario);
                        }
                        return View(lstUsuarios);
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Menu");
                    }
                }
            }
        }

        public ActionResult NovoUsuario()
        {

            return View();
        }

        public ActionResult Edit(int Id)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM usuarios " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var usuario = new Usuario
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = Convert.ToString(dr["nome"]),
                            Celular = Convert.ToString(dr["celular"]),
                            UserName = Convert.ToString(dr["userName"])
                        };
                        return View(usuario);
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Menu");
                    }
                }
            }
        }

        public ActionResult Visualizar(int Id)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM usuarios " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var usuario = new Usuario
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = Convert.ToString(dr["nome"]),
                            Celular = Convert.ToString(dr["celular"]),
                            UserName = Convert.ToString(dr["userName"])
                        };
                        return View(usuario);
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Menu");
                    }
                }
            }
        }

        public ActionResult Excluir(int Id)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM usuarios " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var usuario = new Usuario
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = Convert.ToString(dr["nome"]),
                            Celular = Convert.ToString(dr["celular"]),
                            UserName = Convert.ToString(dr["userName"])
                        };
                        return View(usuario);
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Menu");
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Excluir(Usuario usuario)
        {

            using (var conexao = new Conexao())
            {
                string strLogin = "UPDATE usuarios SET isExcluido = true " +
                                    "where id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@id", usuario.Id);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Menu");
                }
            }


        }

        [HttpPost]
        public ActionResult SalvarAlteracoes(Usuario usuario)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "UPDATE usuarios SET " +
                                    "userName = @userName, " +
                                    "userPass = @userPass, " +
                                    "nome = @nome, " +
                                    "celular = @celular " +
                                    "where id = @Id;";


                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@userName", usuario.UserName);
                    comando.Parameters.AddWithValue("@userPass", usuario.UserPass);
                    comando.Parameters.AddWithValue("@nome", usuario.Nome);
                    comando.Parameters.AddWithValue("@celular", usuario.Celular);
                    comando.Parameters.AddWithValue("@id", usuario.Id);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Menu");
                }
            }
        }

        [HttpPost]
        public ActionResult SalvarUsuario(Usuario usuario)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "INSERT INTO usuarios (userName, userPass, nome, celular) " +
                                  "values (" +
                                  "@userName, @userPass, @nome, @celular);";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@userName", usuario.UserName);
                    comando.Parameters.AddWithValue("@userPass", usuario.UserPass);
                    comando.Parameters.AddWithValue("@nome", usuario.Nome);
                    comando.Parameters.AddWithValue("@celular", usuario.Celular);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Menu");
                }
            }
        }


        public ActionResult EfetuarLogin(Usuario usuario)
        {

            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM usuarios " +
                                  "WHERE userName = @userName and " +
                                  "userPass = @userPass and isExcluido = false;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@userName", usuario.UserName);
                    comando.Parameters.AddWithValue("@userPass", usuario.UserPass);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        return RedirectToAction("Menu");
                    }
                    else
                    {
                        ViewBag.ErroLogin = true;
                        return RedirectToAction("Index");
                    }
                }
            }


        }
    }
}
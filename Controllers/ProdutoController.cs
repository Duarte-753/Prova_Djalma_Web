using MySql.Data.MySqlClient;
using Prova_web.Models;
using System;
using MySql.Data.MySqlClient;
using Prova_web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace Prova_web.Controllers
{
    public class ProdutoController : Controller
    {

        public ActionResult Index(Produto pro)
        {
            var lstMarca = new List<Marca>();
            using (var conexao = new Conexao())
            {
                string strMarca = "SELECT * FROM marcas where isExcluido = false order by marca_M;";
                using (var comando = new MySqlCommand(strMarca, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var marca = new Marca
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Marca_M = Convert.ToString(dr["marca_M"])
                            };

                            lstMarca.Add(marca);
                        }
                    ViewBag.ListaMarcas = lstMarca;
                }
            }

            using (var conexao = new Conexao())
            {

                string strProdutos = "SELECT * FROM produtos " +
                "WHERE Descricao like @descricao and " +
                "isExcluido = false;";

                using (var comando = new MySqlCommand(strProdutos, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@descricao", pro.Descricao + "%");

                    MySqlDataReader dr = comando.ExecuteReader();

                    if (dr.HasRows)
                    {
                        var lstProdutos = new List<Produto>();

                        while (dr.Read())
                        {
                            var produto = new Produto
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Descricao = Convert.ToString(dr["descricao"]),
                                EAN = Convert.ToInt32(dr["ean"]),
                                PreçoVenda = Convert.ToString(dr["precoVenda"]),
                                Marca_P = Convert.ToString(dr["marca_P"]),
                                Estoque = Convert.ToInt32(dr["estoque"]),
                            };

                            lstProdutos.Add(produto);
                        }
                        ViewBag.ListaProdutos = lstProdutos;
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
        public ActionResult EditarProduto(int Id)
        {
            var lstMarca = new List<Marca>();
            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM produtos " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var produto = new Produto
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Descricao = Convert.ToString(dr["descricao"]),
                            EAN = Convert.ToInt32(dr["ean"]),
                            PreçoVenda = Convert.ToString(dr["precoVenda"]),
                            Marca_P = Convert.ToString(dr["marca_P"]),
                            Estoque = Convert.ToInt32(dr["estoque"])

                        };
                        using (var conexao2 = new Conexao())
                        {
                            string strMarca = "SELECT * FROM marcas where isExcluido = false order by marca_M;";
                            using (var comando2 = new MySqlCommand(strMarca, conexao2.conn))
                            {
                                MySqlDataReader dr2 = comando2.ExecuteReader();
                                if (dr2.HasRows)
                                    while (dr2.Read())
                                    {
                                        var marca = new Marca
                                        {
                                            Id = Convert.ToInt32(dr2["Id"]),
                                            Marca_M = Convert.ToString(dr2["marca_M"])
                                        };

                                        lstMarca.Add(marca);
                                    }
                                ViewBag.ListaMarcas = lstMarca;
                            }
                        }
                        return View(produto);
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
        public ActionResult SalvarAlteracoesProduto(Produto produto)
        {

            using (var conexao = new Conexao())
            {
                string strLogin = "UPDATE produtos SET " +
                                    "Descricao = @descricao, " +
                                    "EAN = @ean, " +
                                    "PrecoVenda = @precoVenda, " +
                                    "Marca_P = @marca_P, " +
                                    "Estoque = @estoque " +
                                    "where id = @Id;";


                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@descricao", produto.Descricao);
                    comando.Parameters.AddWithValue("@ean", produto.EAN);
                    comando.Parameters.AddWithValue("@precoVenda", produto.PreçoVenda);
                    comando.Parameters.AddWithValue("@marca_P", produto.Marca_P);
                    comando.Parameters.AddWithValue("@estoque", produto.Estoque);
                    comando.Parameters.AddWithValue("@id", produto.Id);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }
        }

        // Salvar
        public ActionResult NovoProduto()
        {
            var lstMarca = new List<Marca>();
            using (var conexao8 = new Conexao())
            {
                string strMarca = "SELECT * FROM marcas where isExcluido = false order by marca_M;";
                using (var comando8 = new MySqlCommand(strMarca, conexao8.conn))
                {
                    MySqlDataReader dr = comando8.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var marca = new Marca
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Marca_M = Convert.ToString(dr["marca_M"])
                            };

                            lstMarca.Add(marca);
                        }
                    ViewBag.ListaMarcas = lstMarca;
                }

            }
            return View();
        }
        [HttpPost]
        public ActionResult SalvarProduto(Produto produto)
        {
            using (var conexao = new Conexao())
            {
                string strLogin = "INSERT INTO produtos (Descricao, EAN, PrecoVenda, Marca_P, Estoque ) " +
                                  "values (" +
                                  "@descricao, @ean, @precoVenda, @marca_P, @estoque);";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@descricao", produto.Descricao);
                    comando.Parameters.AddWithValue("@ean", produto.EAN);
                    comando.Parameters.AddWithValue("@precoVenda", produto.PreçoVenda); 
                    comando.Parameters.AddWithValue("@marca_P", produto.Marca_P);
                    comando.Parameters.AddWithValue("@estoque", produto.Estoque);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }
        }

        // Visualizar

        public ActionResult VisualizarProduto(int Id)
        {
            var lstMarca = new List<Marca>();
            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM produtos " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var produto = new Produto
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Descricao = Convert.ToString(dr["descricao"]),
                            EAN = Convert.ToInt32(dr["ean"]),
                            PreçoVenda = Convert.ToString(dr["precoVenda"]),
                            Marca_P = Convert.ToString(dr["marca_P"]),
                            Estoque = Convert.ToInt32(dr["estoque"])

                        };
                        using (var conexao2 = new Conexao())
                        {
                            string strMarca = "SELECT * FROM marcas where isExcluido = false order by marca_M;";
                            using (var comando2 = new MySqlCommand(strMarca, conexao2.conn))
                            {
                                MySqlDataReader dr2 = comando2.ExecuteReader();
                                if (dr2.HasRows)
                                    while (dr2.Read())
                                    {
                                        var marca = new Marca
                                        {
                                            Id = Convert.ToInt32(dr2["Id"]),
                                            Marca_M = Convert.ToString(dr2["marca_M"])
                                        };

                                        lstMarca.Add(marca);
                                    }
                                ViewBag.ListaMarcas = lstMarca;
                            }
                        }
                        return View(produto);
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

        public ActionResult ExcluirProduto(int Id)
        {
            var lstMarca = new List<Marca>();
            using (var conexao = new Conexao())
            {
                string strLogin = "SELECT * FROM produtos " +
                                  "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", Id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var produto = new Produto
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Descricao = Convert.ToString(dr["descricao"]),
                            EAN = Convert.ToInt32(dr["ean"]),
                            PreçoVenda = Convert.ToString(dr["precoVenda"]),
                            Marca_P = Convert.ToString(dr["marca_P"]),
                            Estoque = Convert.ToInt32(dr["estoque"])

                        };
                        using (var conexao2 = new Conexao())
                        {
                            string strMarca = "SELECT * FROM marcas where isExcluido = false order by marca_M;";
                            using (var comando2 = new MySqlCommand(strMarca, conexao2.conn))
                            {
                                MySqlDataReader dr2 = comando2.ExecuteReader();
                                if (dr2.HasRows)
                                    while (dr2.Read())
                                    {
                                        var marca = new Marca
                                        {
                                            Id = Convert.ToInt32(dr2["Id"]),
                                            Marca_M = Convert.ToString(dr2["marca_M"])
                                        };

                                        lstMarca.Add(marca);
                                    }
                                ViewBag.ListaMarcas = lstMarca;
                            }
                        }
                        return View(produto);
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
        public ActionResult ExcluirSoftProduto(Produto produto)
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
                string strLogin = "UPDATE produtos SET isExcluido = true " +
                                    "where id = @Id;";

                using (var comando = new MySqlCommand(strLogin, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@id", produto.Id);
                    comando.ExecuteNonQuery();

                    return RedirectToAction("Index");
                }
            }

        }

    }
}
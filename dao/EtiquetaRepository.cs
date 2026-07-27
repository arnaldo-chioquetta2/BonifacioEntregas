using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using TeleBonifacio.tb;

namespace TeleBonifacio.dao
{
    public class EtiquetaRepository
    {
        private static string CaminhoArquivo => Path.Combine(Application.StartupPath, "etiquetas.json");

        public List<EtiquetaModel> Listar()
        {
            try
            {
                GarantirArquivo();

                string json = File.ReadAllText(CaminhoArquivo, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<EtiquetaModel>();
                }

                var etiquetas = JsonConvert.DeserializeObject<List<EtiquetaModel>>(json);
                return etiquetas ?? new List<EtiquetaModel>();
            }
            catch (Exception ex)
            {
                glo.Loga("Erro EtiquetaRepository: " + ex.Message);
                return new List<EtiquetaModel>();
            }
        }

        public void SalvarTodos(List<EtiquetaModel> etiquetas)
        {
            try
            {
                GarantirArquivo();
                string json = JsonConvert.SerializeObject(etiquetas ?? new List<EtiquetaModel>(), Formatting.Indented);
                File.WriteAllText(CaminhoArquivo, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                glo.Loga("Erro EtiquetaRepository: " + ex.Message);
            }
        }

        public void Salvar(EtiquetaModel etiqueta)
        {
            try
            {
                if (etiqueta == null)
                {
                    return;
                }

                var etiquetas = Listar();
                var existente = !string.IsNullOrWhiteSpace(etiqueta.Id)
                    ? etiquetas.FirstOrDefault(x => string.Equals(x.Id, etiqueta.Id, StringComparison.OrdinalIgnoreCase))
                    : null;

                if (string.IsNullOrWhiteSpace(etiqueta.Id))
                {
                    etiqueta.Id = Guid.NewGuid().ToString();
                }

                etiqueta.Fontes = etiqueta.ObterFontesComPadrao();

                if (existente == null)
                {
                    DateTime agora = DateTime.Now;
                    etiqueta.CriadoEm = agora;
                    etiqueta.AlteradoEm = agora;
                    etiquetas.Add(etiqueta);
                }
                else
                {
                    existente.NomeEtiqueta = etiqueta.NomeEtiqueta;
                    existente.ModoTextoLivre = etiqueta.ModoTextoLivre;
                    existente.Codigo = etiqueta.Codigo;
                    existente.Descricao = etiqueta.Descricao;
                    existente.Preco = etiqueta.Preco;
                    existente.Observacao = etiqueta.Observacao;
                    existente.NomeEmpresa = etiqueta.NomeEmpresa;
                    existente.Telefone = etiqueta.Telefone;
                    existente.TeleEntrega = etiqueta.TeleEntrega;
                    existente.Local = etiqueta.Local;
                    existente.Fontes = etiqueta.Fontes == null
                        ? new Dictionary<string, EtiquetaFonteConfig>()
                        : etiqueta.Fontes.ToDictionary(
                            x => x.Key,
                            x => x.Value == null
                                ? null
                                : new EtiquetaFonteConfig
                                {
                                    NomeFonte = x.Value.NomeFonte,
                                    Tamanho = x.Value.Tamanho,
                                    Negrito = x.Value.Negrito
                                });
                    existente.AlteradoEm = DateTime.Now;
                }

                SalvarTodos(etiquetas);
            }
            catch (Exception ex)
            {
                glo.Loga("Erro EtiquetaRepository: " + ex.Message);
            }
        }

        public void Excluir(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return;
                }

                var etiquetas = Listar();
                etiquetas.RemoveAll(x => string.Equals(x.Id, id, StringComparison.OrdinalIgnoreCase));
                SalvarTodos(etiquetas);
            }
            catch (Exception ex)
            {
                glo.Loga("Erro EtiquetaRepository: " + ex.Message);
            }
        }

        public EtiquetaModel BuscarPorId(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return null;
                }

                return Listar().FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                glo.Loga("Erro EtiquetaRepository: " + ex.Message);
                return null;
            }
        }

        public EtiquetaModel BuscarPorCodigo(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    return null;
                }

                return Listar().FirstOrDefault(x => string.Equals(x.Codigo, codigo, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                glo.Loga("Erro EtiquetaRepository: " + ex.Message);
                return null;
            }
        }

        private void GarantirArquivo()
        {
            if (!File.Exists(CaminhoArquivo))
            {
                File.WriteAllText(CaminhoArquivo, "[]", Encoding.UTF8);
            }
        }
    }
}

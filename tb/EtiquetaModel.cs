using System;
using System.Collections.Generic;

namespace TeleBonifacio.tb
{
    public class EtiquetaFonteConfig
    {
        public string NomeFonte { get; set; }
        public float Tamanho { get; set; }
        public bool Negrito { get; set; }
    }

    public class EtiquetaTextoLivreLinha
    {
        public string Id { get; set; }
        public string Texto { get; set; }
        public EtiquetaFonteConfig Fonte { get; set; }
    }

    public class EtiquetaModel
    {
        public string Id { get; set; }
        public bool ModoTextoLivre { get; set; }
        public string NomeEtiqueta { get; set; }
        public string NomeEmpresa { get; set; }
        public string Telefone { get; set; }
        public string TeleEntrega { get; set; }
        public string Local { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Preco { get; set; }
        public string Observacao { get; set; }
        public List<EtiquetaTextoLivreLinha> LinhasTextoLivre { get; set; } = new List<EtiquetaTextoLivreLinha>();
        public Dictionary<string, EtiquetaFonteConfig> Fontes { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AlteradoEm { get; set; }

        public void SincronizarLinhasTextoLivre()
        {
            if (!ModoTextoLivre || string.IsNullOrEmpty(Observacao) ||
                (LinhasTextoLivre != null && LinhasTextoLivre.Count > 0))
            {
                return;
            }

            Dictionary<string, EtiquetaFonteConfig> fontesPadrao = ObterFontesComPadrao();
            EtiquetaFonteConfig fonteObservacao = fontesPadrao["Observacao"];
            string[] linhas = Observacao.Split(
                new[] { "\r\n", "\n", "\r" },
                StringSplitOptions.None);

            LinhasTextoLivre = new List<EtiquetaTextoLivreLinha>();
            foreach (string linha in linhas)
            {
                LinhasTextoLivre.Add(new EtiquetaTextoLivreLinha
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Texto = linha,
                    Fonte = CopiarFonte(fonteObservacao)
                });
            }
        }

        public static List<EtiquetaTextoLivreLinha> CopiarLinhasTextoLivre(
            List<EtiquetaTextoLivreLinha> linhas)
        {
            if (linhas == null)
            {
                return new List<EtiquetaTextoLivreLinha>();
            }

            var copia = new List<EtiquetaTextoLivreLinha>();
            foreach (EtiquetaTextoLivreLinha linha in linhas)
            {
                copia.Add(linha == null
                    ? null
                    : new EtiquetaTextoLivreLinha
                    {
                        Id = linha.Id,
                        Texto = linha.Texto,
                        Fonte = CopiarFonte(linha.Fonte)
                    });
            }

            return copia;
        }

        private static EtiquetaFonteConfig CopiarFonte(EtiquetaFonteConfig fonte)
        {
            return fonte == null
                ? null
                : new EtiquetaFonteConfig
                {
                    NomeFonte = fonte.NomeFonte,
                    Tamanho = fonte.Tamanho,
                    Negrito = fonte.Negrito
                };
        }
        public Dictionary<string, EtiquetaFonteConfig> ObterFontesComPadrao()
        {
            var fontes = Fontes ?? new Dictionary<string, EtiquetaFonteConfig>();

            return new Dictionary<string, EtiquetaFonteConfig>
            {
                { "NomeEmpresa", NormalizarFonte(fontes, "NomeEmpresa", "Arial", 15f, true) },
                { "Telefone", NormalizarFonte(fontes, "Telefone", "Arial", 15f, true) },
                { "Codigo", NormalizarFonte(fontes, "Codigo", "Arial", 8f, true) },
                { "Descricao", NormalizarFonte(fontes, "Descricao", "Arial", 7f, false) },
                { "Preco", NormalizarFonte(fontes, "Preco", "Arial", 15f, true) },
                { "Observacao", NormalizarFonte(fontes, "Observacao", "Arial", 7f, false) },
                { "TeleEntrega", NormalizarFonte(fontes, "TeleEntrega", "Arial", 7f, true) },
                { "Local", NormalizarFonte(fontes, "Local", "Arial", 7f, true) }
            };
        }

        private static EtiquetaFonteConfig NormalizarFonte(
            Dictionary<string, EtiquetaFonteConfig> fontes,
            string chave,
            string nomeFontePadrao,
            float tamanhoPadrao,
            bool negritoPadrao)
        {
            EtiquetaFonteConfig fonte;

            if (fontes == null || !fontes.TryGetValue(chave, out fonte) || fonte == null)
            {
                return new EtiquetaFonteConfig
                {
                    NomeFonte = nomeFontePadrao,
                    Tamanho = tamanhoPadrao,
                    Negrito = negritoPadrao
                };
            }

            return new EtiquetaFonteConfig
            {
                NomeFonte = string.IsNullOrWhiteSpace(fonte.NomeFonte) ? nomeFontePadrao : fonte.NomeFonte,
                Tamanho = fonte.Tamanho <= 0 ? tamanhoPadrao : fonte.Tamanho,
                Negrito = fonte.Negrito
            };
        }
    }
}

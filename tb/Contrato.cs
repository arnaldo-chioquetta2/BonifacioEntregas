﻿using System;
using System.Collections.Generic;

namespace TeleBonifacio.tb
{
    public class Contrato : IDataEntity
    {        

        public int Id { get; set; } // Identificador único do contrato
        public bool Adicao { get; set; } // Indica se é uma nova adição ou edição

        public string Descricao { get; set; } // Descrição do contrato
        public int IdEntregador { get; set; } // ID do entregador vinculado ao contrato
        public decimal Valor { get; set; } // Valor do contrato
        public string Status { get; set; } // Status do contrato (Ativo, Cancelado, Finalizado)
        public DateTime DataInicio { get; set; } // Data de início do contrato
        public DateTime DataTermino { get; set; } // Data de término do contrato
        public string Pix { get; set; } // Dados PIX do contratado
        public string Observacoes { get; set; } // Observações adicionais
        public string Nome { get; set; }

        public string Contratante { get; set; }
        public string ContratanteCNPJ { get; set; }
        public string ContratanteEndereco { get; set; }
        public string Contratada { get; set; }
        public string ContratadaCNPJ { get; set; }
        public string ContratadaEndereco { get; set; }
        public string NomeEmpresa { get; set; } 
        public string CNPJEmpresa { get; set; }

        public List<string> Clausulas { get; set; }

        public int tpContrato { get; set; }

    }
}

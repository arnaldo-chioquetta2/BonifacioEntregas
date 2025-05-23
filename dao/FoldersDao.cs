﻿using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace TeleBonifacio.dao
{
    public class FoldersDao
    {

        public List<tb.Folder> GetRootFolders()
        {
            List<tb.Folder> rootFolders = new List<tb.Folder>();
            string queryCheck = "SELECT COUNT(*) FROM Folders";
            int countNome = DB.ExecutarConsultaCount(queryCheck);
            if (countNome==0)
            {
                InitializeFolders();
            }
            // Agora buscar as pastas raiz
            string query = "SELECT FolderID, FolderName FROM Folders WHERE ParentFolderID IS NULL";

            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rootFolders.Add(new tb.Folder
                            {
                                FolderID = reader.GetInt32(0),
                                FolderName = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return rootFolders;
        }

        public List<tb.Folder> GetSubFolders(int parentFolderId)
        {
            List<tb.Folder> subFolders = new List<tb.Folder>();
            string query = "SELECT FolderID, FolderName FROM Folders WHERE ParentFolderID = " + parentFolderId.ToString();

            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ParentFolderID", parentFolderId);
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subFolders.Add(new tb.Folder
                            {
                                FolderID = reader.GetInt32(0),
                                FolderName = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return subFolders;
        }

        public List<tb.Document> GetDocuments(int folderId)
        {
            List<tb.Document> documents = new List<tb.Document>();
            string query = "SELECT ID, CaminhoPDF, FolderID, idArquivo FROM ContasAPagar WHERE FolderID = @FolderID";
            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FolderID", folderId);
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tb.Document Doc = new tb.Document();
                            Doc.DocumentID = reader.GetInt32(0);
                            Doc.DocumentName = reader.GetString(1);
                            Doc.FolderID = reader.GetInt32(2);
                            if (!reader.IsDBNull(3))
                            {
                                Doc.idArquivo = reader.GetInt32(3); 
                            }
                            else
                            {
                                Doc.idArquivo = 0; 
                            }
                            documents.Add(Doc);
                        }                        
                    }
                }
            }
            return documents;
        }

        #region Popula

        public void InitializeFolders()
        {
            int permanentesFolderId = EnsureFolderExists("Permanentes");
            int temporariosFolderId = EnsureFolderExists("Temporários");

            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                string query = @"
            UPDATE ContasAPagar 
            SET FolderID = IIF(Perm = True, @PermanentesFolderId, @TemporariosFolderId)";

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PermanentesFolderId", permanentesFolderId);
                    command.Parameters.AddWithValue("@TemporariosFolderId", temporariosFolderId);
                    command.ExecuteNonQuery();
                }
            }
        }

        private int EnsureFolderExists(string folderName)
        {
            int folderId = 0;

            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();

                // Verifica se a pasta já existe
                string selectQuery = "SELECT FolderID FROM Folders WHERE FolderName = @FolderName AND ParentFolderID IS NULL";
                using (OleDbCommand selectCommand = new OleDbCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@FolderName", folderName);
                    var result = selectCommand.ExecuteScalar();
                    if (result != null)
                    {
                        folderId = (int)result;
                    }
                    else
                    {
                        // Caso não exista, insere a nova pasta
                        string insertQuery = "INSERT INTO Folders (FolderName, ParentFolderID) VALUES (@FolderName, NULL)";
                        using (OleDbCommand insertCommand = new OleDbCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@FolderName", folderName);
                            insertCommand.ExecuteNonQuery();

                            // Recupera o ID da pasta recém-criada
                            insertCommand.CommandText = "SELECT @@IDENTITY";
                            folderId = (int)(insertCommand.ExecuteScalar());
                        }
                    }
                }
            }

            return folderId;
        }

        public List<tb.Document> GetFilteredDocuments(int folderId, int idForne, DateTime? dataPagamento, DateTime? dataVencimento, DateTime? dataEmissao, string valorTotal, string descricao, string observacoes, bool? pago)
        {
            List<tb.Document> documents = new List<tb.Document>();
            // Construindo a consulta SQL
            string query = "SELECT ID, CaminhoPDF, FolderID, idArquivo FROM ContasAPagar WHERE FolderID = @FolderID";

            // Adicionar condições conforme os parâmetros não forem nulos
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                query += " AND Descricao LIKE @Descricao";
            }

            using (OleDbConnection connection = new OleDbConnection(glo.connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FolderID", folderId);

                    if (!string.IsNullOrWhiteSpace(descricao))
                    {
                        command.Parameters.AddWithValue("@Descricao", "%" + descricao + "%");
                    }

                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tb.Document doc = new tb.Document();
                            doc.DocumentID = reader.GetInt32(0);
                            doc.DocumentName = reader.GetString(1);
                            doc.FolderID = reader.GetInt32(2);

                            if (!reader.IsDBNull(3))
                            {
                                doc.idArquivo = reader.GetInt32(3);
                            }
                            else
                            {
                                doc.idArquivo = 0;
                            }

                            documents.Add(doc);
                        }
                    }
                }
            }
            return documents;
        }



        #endregion

    }
}

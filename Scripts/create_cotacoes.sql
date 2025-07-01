CREATE TABLE Cotacoes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Descricao VARCHAR(255) NOT NULL,
    FornecedorId INT NOT NULL,
    Valor DECIMAL(10,2) NOT NULL,
    Aprovacao VARCHAR(50) NOT NULL,
    FOREIGN KEY (FornecedorId) REFERENCES Fornecedores(Id)
);

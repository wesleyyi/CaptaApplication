--cria banco de dados
CREATE DATABASE CaptaApplication;

--referencia ao banco de dados
use CaptaApplication

--Cria tabela principal clientes
Create table clientes
(
	id int IDENTITY(1,1) primary key,
    nome varchar(80),
    cpf  varchar(30) NOT NULL,
	sexo  varchar(30),
);

CREATE UNIQUE INDEX cpf
 ON clientes (cpf);

--Cria tabela tipo_cliente que faz referencia aos dados da tabela clientes
Create table tipo_cliente
(
id int IDENTITY(1,1) primary key,
id_cliente int NOT NULL,
tipo_cliente varchar(30)
);

ALTER TABLE tipo_cliente
   ADD CONSTRAINT FK_id_cliente_tipo_cliente FOREIGN KEY (id_cliente)
      REFERENCES clientes (id)
      ON DELETE CASCADE
      ON UPDATE CASCADE
;

CREATE UNIQUE INDEX id_cliente_tipo_cliente
 ON tipo_cliente (id_cliente);

--Cria tabela situacao_cliente que faz referencia aos dados da tabela clientes
Create table situacao_cliente
(
id int IDENTITY(1,1) primary key,
id_cliente int NOT NULL,
situacao_cliente varchar(30)
);

ALTER TABLE situacao_cliente
   ADD CONSTRAINT FK_id_cliente_situacao_cliente FOREIGN KEY (id_cliente)
      REFERENCES clientes (id)
      ON DELETE CASCADE
      ON UPDATE CASCADE
;

CREATE UNIQUE INDEX id_cliente_situacao_cliente
 ON situacao_cliente (id_cliente);







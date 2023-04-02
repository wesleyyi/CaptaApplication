DECLARE @nome VARCHAR(80);
DECLARE @cpf VARCHAR(80);
DECLARE @sexo VARCHAR(80);
DECLARE @tipo_cliente VARCHAR(80);
DECLARE @situacao_cliente VARCHAR(80);
DECLARE @id_cliente int;

--Mudar valores para inserir
select @nome = 'Wesley', @cpf = '458.534.177-17', @sexo = 'Masculino', @tipo_cliente = 'Candidato', @situacao_cliente = 'Trabalhando'

INSERT clientes(nome, cpf, sexo) VALUES(@nome, @cpf, @sexo)

SELECT @id_cliente = id from clientes where cpf = @cpf

INSERT tipo_cliente(id_cliente, tipo_cliente) values(@id_cliente, @tipo_cliente)

INSERT situacao_cliente(id_cliente, situacao_cliente) values(@id_cliente, @situacao_cliente)


--select * from clientes
--select * from situacao_cliente
--select * from tipo_cliente

--delete clientes;
--delete situacao_cliente;
--delete tipo_cliente;




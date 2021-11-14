CREATE TABLE mail (
	id INTEGER auto_increment NOT NULL,
	`from` VARCHAR(200) NOT NULL,
	`to` varchar(200) NOT NULL,
	subject varchar(300) NOT NULL,
	body TEXT NULL,
	CONSTRAINT mail_PK PRIMARY KEY (id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8
COLLATE=utf8_general_ci;
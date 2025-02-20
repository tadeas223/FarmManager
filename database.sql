/* This sql was created by me, but formatted by ChatGPT */
CREATE TABLE Associations (
    idAssociation INT PRIMARY KEY IDENTITY(1, 1),
    name NVARCHAR(50) NOT NULL
);

CREATE TABLE Crops (
    idCrop INT PRIMARY KEY IDENTITY(1, 1),
    name NVARCHAR(50) NOT NULL,
    price FLOAT NOT NULL,
    CHECK (price > 0)
);

CREATE TABLE Fields (
    idField INT PRIMARY KEY IDENTITY(1, 1),
    idAssociation INT NOT NULL,
    name NVARCHAR(50) NOT NULL,
    size FLOAT NOT NULL,
    FOREIGN KEY (idAssociation) REFERENCES Associations(idAssociation),
    CHECK (size > 0)
);

CREATE TABLE Workers (
    idWorker INT PRIMARY KEY IDENTITY(1, 1),
    idAssociation INT NOT NULL,
    name NVARCHAR(50) NOT NULL,
    surname NVARCHAR(50) NOT NULL,
    birthDate DATE NOT NULL,
    fired BIT NOT NULL,
    FOREIGN KEY (idAssociation) REFERENCES Associations(idAssociation)
);

CREATE TABLE SowingRecords (
    idSowingRecord INT PRIMARY KEY IDENTITY(1, 1),
    idCrop INT NOT NULL,
    idField INT NOT NULL,
    idWorker INT NOT NULL,
    date DATE NOT NULL,
    FOREIGN KEY (idCrop) REFERENCES Crops(idCrop),
    FOREIGN KEY (idField) REFERENCES Fields(idField),
    FOREIGN KEY (idWorker) REFERENCES Workers(idWorker)
);

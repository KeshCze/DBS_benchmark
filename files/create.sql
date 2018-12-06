CREATE TABLE FileType (
  id            INT PRIMARY KEY,
  extension     VARCHAR(5) NOT NULL,
  mime          VARCHAR(20) NOT NULL);
  
CREATE TABLE PaperVersion (
  id                   INT PRIMARY KEY,
  uploadDate           DATE NOT NULL,
  idFileType           INT NOT NULL References FileType);

-------------------------------------------------------------------------------
INSERT INTO FileType Values(1, 'pdf', 'application/pdf');
INSERT INTO FileType Values(2, 'ps', 'application/ps');
INSERT INTO FileType Values(3, 'word', 'application/word');

INSERT INTO PaperVersion Values(1, '2/12/2008', 1);
INSERT INTO PaperVersion Values(2, '5/12/2008', 1);
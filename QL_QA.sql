USE master
IF EXISTS(SELECT * FROM sys.databases WHERE name='QL_QA')
BEGIN
        DROP DATABASE QL_QA
END
CREATE DATABASE QL_QA
GO
USE QL_QA
GO

CREATE TABLE QUYEN 
(
    MAQUYEN INT IDENTITY NOT NULL,
    TENQUYEN NVARCHAR(50), -- TÊN CÁC NHÓM QUYỀN(ADIMIN,NHÂN VIÊN)
    CONSTRAINT PK_QUYEN PRIMARY KEY (MAQUYEN) --KHÓA CHÍNH
)
CREATE TABLE TAIKHOAN 
(
    MATK CHAR(10) NOT NULL, 
    TENTK VARCHAR(50),
	MATKHAU CHAR(50),
    MAQUYEN INT ,
	--KHÓA CHÍNH
	CONSTRAINT PK_TAIKHOAN PRIMARY KEY (MATK),
	--KHÓA NGOẠI
	CONSTRAINT FK_TK_QUYEN FOREIGN KEY(MAQUYEN) REFERENCES QUYEN(MAQUYEN)
)
CREATE TABLE THONGTINTAIKHOAN 
(
	MATK CHAR(10) NOT NULL, 
    HOTEN NVARCHAR(50), -- HỌ TÊN
    NGSINH DATE, -- NGÀY SINH
    GTINH NVARCHAR(3),
    NGTAO DATETIME, -- NGÀY TẠO
    EMAIL VARCHAR(50), -- ĐỊA CHỈ EMAIL
    SDT VARCHAR(11), -- SDT
    DCHI NVARCHAR(50), -- ĐỊA CHỈ NHÀ
	TINHTRANG NVARCHAR(50),---ĐANG LÀM ,NGHỈ VIỆC
	--KHÓA CHÍNH
    CONSTRAINT PK_TTTK PRIMARY KEY (MATK),
	--KHÓA NGOẠI
	CONSTRAINT FK_TTTK_TAIKHOAN FOREIGN KEY(MATK) REFERENCES TAIKHOAN(MATK)
)
CREATE TABLE KHACHHANG 
(
    MAKH VARCHAR(11) NOT NULL, -- sdt 
    DIEMTICHLUY INT,
    CONSTRAINT PK_KH PRIMARY KEY (MAKH)
)
CREATE TABLE NHANVIEN 
(
    MANV VARCHAR(10) NOT NULL,
    MATK CHAR(10) NOT NULL, 
    CONSTRAINT PK_NV PRIMARY KEY (MANV),
    CONSTRAINT FK_NV_TK FOREIGN KEY (MATK) REFERENCES TAIKHOAN(MATK)
)
CREATE TABLE LOAISP 
(
    MALSP VARCHAR(6) NOT NULL,
    TENLOAI NVARCHAR(50),
    CONSTRAINT PK_LSP PRIMARY KEY (MALSP)
)
CREATE TABLE SANPHAM 
(
    MASP INT IDENTITY NOT NULL, -- CREATE AUTO
    TENSP NVARCHAR(MAX), -- TÊN SẢN PHẨM
    DONGIA FLOAT, -- ĐƠN GIÁ
	HINHANH NVARCHAR(MAX),--HÌNH ẢNH SẢN PHẨM
    NSX NVARCHAR(30), -- NHÀ SẢN XUẤT
    MALSP VARCHAR(6) REFERENCES LOAISP(MALSP),
    CONSTRAINT PK_SP PRIMARY KEY (MASP)
)
CREATE TABLE KHO 
(
	MASP INT NOT NULL REFERENCES SANPHAM(MASP),
    SIZE VARCHAR(10), -- KÍCH THƯỚC
    SOLUONG INT, -- SỐ LƯỢNG TỒN KHO
	--KHÓA CHÍNH
    CONSTRAINT PK_KHO PRIMARY KEY (MASP,SIZE),
	--KHÓA NGOẠI
	CONSTRAINT FK_KHO_SANPHAM FOREIGN KEY(MASP) REFERENCES SANPHAM(MASP)
)
CREATE TABLE HOADON 
(
    MAHD INT IDENTITY NOT NULL, -- CREATE AUTO
    NGTAO DATETIME, -- NGÀY TẠO HÓA ĐƠN
    THANHTOAN FLOAT, -- TỔNG (SỐ LƯỢNG * ĐƠN GIÁ)
    TINHTRANG NVARCHAR(50), -- CHƯA THANHTOAN,DATT
    MAKH VARCHAR(11) REFERENCES KHACHHANG(MAKH),
    MANV VARCHAR(10) REFERENCES NHANVIEN(MANV),
    CONSTRAINT PK_HD PRIMARY KEY (MAHD)
)
CREATE TABLE CHITIETHD 
(
    MAHD INT NOT NULL,
    MASP INT NOT NULL,
	SIZE VARCHAR(10),
    SOLUONG INT, -- SỐ LƯỢNG > 0, số lượng bán
	--KHÓA CHÍNH
    CONSTRAINT PK_CTHD PRIMARY KEY (MAHD,MASP,SIZE),
	--KHÓA NGOẠI
	CONSTRAINT FK_CTHD_KHO FOREIGN KEY(MASP,SIZE) REFERENCES KHO(MASP,SIZE),
	CONSTRAINT FK_CTHD_HOADON FOREIGN KEY(MAHD) REFERENCES HOADON(MAHD)
)


CREATE TABLE THONGKE 
(
    THANG int NOT NULL,
	NAM int NOT NULL,
	DOANHTHU int,
	--KHÓA CHÍNH
    CONSTRAINT PK_THONGKE PRIMARY KEY (THANG,NAM) 
)
----------------------------------------------------------------------------
--RÀNG BUỘC TOÀN VẸN
----------------------------------------------------------------------------
SET DATEFORMAT DMY

--TABLE TAIKHOAN
ALTER TABLE TAIKHOAN ADD CONSTRAINT uni_TenLoaiTK UNIQUE(TENTK)
GO

--TABLE THONGTINTAIKHOAN
ALTER TABLE THONGTINTAIKHOAN ADD CONSTRAINT chk_GioiTinh CHECK(GTINH IN (N'Nam', N'Nữ'))
ALTER TABLE THONGTINTAIKHOAN ADD CONSTRAINT chk_NgayVaoLam CHECK(NGTAO<=GETDATE())
ALTER TABLE THONGTINTAIKHOAN ADD CONSTRAINT DEF_DC DEFAULT N'TP.HCM' FOR DCHI
ALTER TABLE THONGTINTAIKHOAN ADD CONSTRAINT chk_SDT_NV CHECK(LEN(SDT)=10)
ALTER TABLE THONGTINTAIKHOAN ADD CONSTRAINT UNI_SDT UNIQUE(SDT)
ALTER TABLE THONGTINTAIKHOAN ADD CONSTRAINT UNI_EMAIL UNIQUE(EMAIL)
GO

--TABLE KHACHHANG
ALTER TABLE KHACHHANG ADD CONSTRAINT chk_SDT_KH CHECK(LEN(MAKH)=10)
ALTER TABLE KHACHHANG ADD CONSTRAINT chk_PhanTramGiam CHECK(DIEMTICHLUY>=0 AND DIEMTICHLUY<=50)
GO

--TABLE HOADON
ALTER TABLE CHITIETHD ADD CONSTRAINT chk_SoLuong_CTHD CHECK(SOLUONG>0)
ALTER TABLE HOADON ADD CONSTRAINT chk_ThanhTien_HD CHECK(THANHTOAN>0)
GO

--TABLE SANPHAM

ALTER TABLE SANPHAM ADD CONSTRAINT chk_GiaBan CHECK(DONGIA>0)
GO

--Ràng buộc trigger

--Cập nhật thanh toán trên hóa đơn
CREATE TRIGGER CAPNHATTHANHTOAN
ON CHITIETHD
FOR INSERT,UPDATE,DELETE
AS
	BEGIN
	DECLARE @GiamGia MONEY
	--cập nhật TỔNG TIỀN sau khi trừ đi ĐIỂM TÍCH LŨY cho KHACHHANG (1 điểm được 20000)
	IF((SELECT DIEMTICHLUY FROM KHACHHANG,HOADON WHERE HOADON.MAKH=KHACHHANG.MAKH AND MAHD =(SELECT MAHD FROM inserted))>0)
	BEGIN
		SET @GiamGia = (SELECT DIEMTICHLUY FROM KHACHHANG,HOADON WHERE HOADON.MAKH=KHACHHANG.MAKH AND MAHD =(SELECT MAHD FROM inserted))*20000
	-- cập nhật TỔNG TIỀN thu được của mỗi CHITIETHD
		UPDATE HOADON
		SET THANHTOAN = (SELECT SUM(CHITIETHD.SOLUONG*SANPHAM.DONGIA)
						FROM CHITIETHD,SANPHAM
						WHERE CHITIETHD.MASP = SANPHAM.MASP AND CHITIETHD.MAHD =(SELECT MAHD FROM inserted) GROUP BY CHITIETHD.MAHD) - @GiamGia
		WHERE MAHD =(SELECT MAHD FROM inserted)
	END
		ELSE
		-- cập nhật TỔNG TIỀN thu được của mỗi CHITIETHD
		UPDATE HOADON
		SET THANHTOAN = (SELECT SUM(CHITIETHD.SOLUONG*SANPHAM.DONGIA)
						FROM CHITIETHD,SANPHAM
						WHERE CHITIETHD.MASP = SANPHAM.MASP AND CHITIETHD.MAHD =(SELECT MAHD FROM inserted) GROUP BY CHITIETHD.MAHD)
		WHERE MAHD =(SELECT MAHD FROM inserted)
	END		
GO
--cập nhật điểm tích lũy khi khách hàng mua sản phẩm trên 500k

CREATE TRIGGER sp_UpdateDiemTL
ON HOADON
AFTER UPDATE
AS
	IF((SELECT THANHTOAN FROM HOADON WHERE MAHD =(SELECT MAHD FROM inserted)) > 500000 
		AND (SELECT TINHTRANG FROM HOADON WHERE MAHD =(SELECT MAHD FROM inserted)) = N'Đã thanh toán')
	BEGIN
		UPDATE KHACHHANG 
		SET DIEMTICHLUY = DIEMTICHLUY + 1
		WHERE MAKH =(SELECT MAKH FROM inserted)
	END
GO


----------------------------------------------------------------------------
--NHẬP DỮ LIỆU
----------------------------------------------------------------------------


--BẢNG QUYỀN
INSERT QUYEN VALUES(N'ADMIN')
INSERT QUYEN VALUES(N'NHÂN VIÊN')

--BẢNG TÀI KHOẢN
CREATE PROC sp_AddTK(
	@MATK CHAR(10) , 
	@TENTK VARCHAR(50),
	@MATKHAU CHAR(50),
    @MAQUYEN INT)

AS 
--KIỂM TRA TRÙNG KHÓA CHÍNH
	IF EXISTS(SELECT * FROM TAIKHOAN WHERE MATK=@MATK)
		RETURN 0
	--THÊM MỚI DỮ LIỆU
	INSERT INTO TAIKHOAN(MATK,TENTK,MATKHAU,MAQUYEN)
	VALUES(@MATK,@TENTK, @MATKHAU, @MAQUYEN)
GO
--mã hash=123
EXEC sp_AddTK '1','ADMIN1','123','1'
EXEC sp_AddTK '2','ADMIN2','123','1'
EXEC sp_AddTK '3','NHANVIEN1','123','2'
EXEC sp_AddTK '4','NHANVIEN2','123','2'
EXEC sp_AddTK '5','NHANVIEN3','123','2'
EXEC sp_AddTK '6','NHANVIEN4','123','2'

--BẢNG THÔNG TIN TÀI KHOẢN
CREATE PROC sp_AddTTTK(
	@MATK CHAR(10) , 
    @HOTEN NVARCHAR(50), -- HỌ TÊN
    @NGSINH DATE, -- NGÀY SINH
    @GTINH NVARCHAR(3),
    @NGTAO DATETIME, -- NGÀY TẠO
    @EMAIL VARCHAR(50), -- ĐỊA CHỈ EMAIL
    @SDT VARCHAR(11), -- SDT
    @DCHI NVARCHAR(50),
	@TINHTRANG NVARCHAR(50))

AS 
--KIỂM TRA TRÙNG KHÓA CHÍNH
	IF EXISTS(SELECT * FROM THONGTINTAIKHOAN WHERE MATK=@MATK)
		RETURN 0
	--THÊM MỚI DỮ LIỆU
	INSERT INTO THONGTINTAIKHOAN(MATK,HOTEN,NGSINH,GTINH,NGTAO,EMAIL,SDT,DCHI,TINHTRANG)
	VALUES(@MATK,@HOTEN,@NGSINH,@GTINH,@NGTAO,@EMAIL,@SDT,@DCHI,@TINHTRANG)
GO
SET DATEFORMAT DMY
EXEC sp_AddTTTK '1',N'LÊ THÙY NA','17/05/2001',N'Nữ','1/1/2021 07:08:09','nalethuy@gmail.com','0362417182',N'Phú Yên',N'Đang Làm'
EXEC sp_AddTTTK '2',N'TRẦN TRỌNG BÌNH PHƯƠNG','01/01/2001',N'NAM','1/1/2021 07:08:09','phuong@gmail.com','0123456789',N'Đồng Tháp',N'Đang Làm'
EXEC sp_AddTTTK '3',N'TRẦN ĐỨC THẮNG','09/07/2001',N'Nam','1/1/2021 07:08:09','bnguyenva@gmail.com','0345674123',N'Bình Dương',N'Đang Làm'
EXEC sp_AddTTTK '4',N'HUỲNH THU PHƯƠNG','08/07/1999',N'Nữ','1/1/2021 07:08:09','tunu@gmail.com','0312345678',N'Bình Dương',N'Đang Làm'
EXEC sp_AddTTTK '5',N'TẠ MINH TRỰC','05/07/2001',N'Nam','1/1/2021 07:08:09','minhtruc12@gmail.com','0383039079',N'Bình Dương',N'Đang Làm'
EXEC sp_AddTTTK '6',N'VÕ KIỀU MINH HẢI','01/07/1999',N'Nữ','1/1/2021 07:08:09','minhhai2@gmail.com','0961314900',N'Bình Dương',N'Đang Làm'

--BẢNG KHÁCH HÀNG
INSERT KHACHHANG VALUES('0934214565',0)
INSERT KHACHHANG VALUES('0948571923',1)
INSERT KHACHHANG VALUES('0968492918',2)
INSERT KHACHHANG VALUES('0989090761',3)

--BẢNG NHÂN VIÊN
INSERT NHANVIEN VALUES('NV01','3')
INSERT NHANVIEN VALUES('NV02','4')
INSERT NHANVIEN VALUES('NV03','5')
INSERT NHANVIEN VALUES('NV04','6')

--BẢNG THỐNG KÊ
INSERT INTO THONGKE VALUES
(1,2021,900000),
(1,2022,800000),
(2,2021,900000),
(2,2022,600000),
(3,2021,700000),
(3,2022,500000),
(4,2021,900000),
(4,2022,100000),
(5,2021,800000),
(5,2022,600000),
(6,2021,800000),
(6,2022,10000),
(7,2021,900000),
(8,2021,900000),
(9,2021,300000),
(10,2021,900000),
(11,2021,600000),
(12,2021,200000)

-- BẢNG LOẠI SP
-- áo
INSERT LOAISP VALUES('LSP01',N'Trễ vai')
INSERT LOAISP VALUES('LSP02',N'Croptop')
INSERT LOAISP VALUES('LSP03',N'Sơ mi')
INSERT LOAISP VALUES('LSP04',N'Hai dây')

-- quần
INSERT LOAISP VALUES('LSP05',N'Quần short')
INSERT LOAISP VALUES('LSP06',N'Quần ống loe')
INSERT LOAISP VALUES('LSP07',N'Quần skinny')
INSERT LOAISP VALUES('LSP08',N'Quần giả váy')

-- váy
INSERT LOAISP VALUES('LSP09',N'Váy tennis')
INSERT LOAISP VALUES('LSP10',N'Váy chữ A')
INSERT LOAISP VALUES('LSP11',N'Váy jeans')
INSERT LOAISP VALUES('LSP12',N'Váy xòe')

-- đầm
INSERT LOAISP VALUES('LSP13',N'Đầm body')
INSERT LOAISP VALUES('LSP14',N'Đầm babydoll')
INSERT LOAISP VALUES('LSP15',N'Đầm trễ vai')
INSERT LOAISP VALUES('LSP16',N'Set')

CREATE PROC sp_AddSP(
	@TENSP NVARCHAR(MAX), -- TÊN SẢN PHẨM
    @DONGIA FLOAT, -- ĐƠN GIÁ
	@HINHANH NVARCHAR(MAX),
    @NSX NVARCHAR(30), -- NHÀ SẢN XUẤT
    @MALSP VARCHAR(6))

AS 
--KIỂM TRA TRÙNG KHÓA CHÍNH
	IF EXISTS(SELECT * FROM SANPHAM WHERE TENSP=@TENSP)
		RETURN 0
	--THÊM MỚI DỮ LIỆU
	INSERT INTO SANPHAM(TENSP,DONGIA,HINHANH,NSX,MALSP)
	VALUES(@TENSP, @DONGIA,@HINHANH,@NSX,@MALSP)
GO
--BẢNG SP
--ÁO
--trễ vai
EXEC sp_AddSP N'Áo trễ vai 1',650000,'1.jpg',N'Việt Nam','LSP01'
EXEC sp_AddSP N'Áo trễ vai 2',280000,'2.jpg',N'Trung Quốc','LSP01'
EXEC sp_AddSP N'Áo trễ vai 3',460000,'3.jpg',N'Hàn Quốc','LSP01'
EXEC sp_AddSP N'Áo trễ vai 4',550000,'4.jpg',N'Thái Lan','LSP01'
EXEC sp_AddSP N'Áo trễ vai 5',280000,'5.jpg',N'Việt Nam','LSP01'

--áo croptop

EXEC sp_AddSP N'Áo croptop 1',650000,'6.jpg',N'Việt Nam','LSP02'
EXEC sp_AddSP N'Áo croptop 2',740000,'7.jpg',N'Mỹ','LSP02'
EXEC sp_AddSP N'Áo croptop 3',320000,'8.jpg',N'Nhật','LSP02'
EXEC sp_AddSP N'Áo croptop 4',450000,'9.jpg',N'Hàn Quốc','LSP02'
EXEC sp_AddSP N'Áo croptop 5',650000,'10.jpg',N'Thái Lan','LSP02'
--áo sơ mi

EXEC sp_AddSP N'Áo sơ mi 1',400000,'11.jpg',N'Việt Nam','LSP03'
EXEC sp_AddSP N'Áo sơ mi 2',550000,'12.jpg',N'Trung Quốc','LSP03'
EXEC sp_AddSP N'Áo sơ mi 3',290000,'13.jpg',N'Hàn Quốc','LSP03'
EXEC sp_AddSP N'Áo sơ mi 4',620000,'14.jpg',N'Nhật Bản','LSP03'
EXEC sp_AddSP N'Áo sơ mi 5',500000,'15.jpg',N'Việt Nam','LSP03'
--hai dây
EXEC sp_AddSP N'Áo hai dây 1',440000,'16.jpg',N'Hàn Quốc','LSP04'
EXEC sp_AddSP N'Áo hai dây 2',590000,'17.jpg',N'Việt Nam','LSP04'
EXEC sp_AddSP N'Áo hai dây 3',780000,'18.jpg',N'Mỹ','LSP04'
EXEC sp_AddSP N'Áo hai dây 4',430000,'19.jpg',N'Việt Nam','LSP04'
EXEC sp_AddSP N'Áo hai dây 5',390000,'20.jpg',N'Anh','LSP04'

--QUẦN
--quần short
EXEC sp_AddSP N'Quần short 1',560000,'21.jpg',N'Nhật','LSP05'
EXEC sp_AddSP N'Quần short 2',650000,'22.jpng',N'Việt Nam','LSP05'
EXEC sp_AddSP N'Quần short 3',340000,'23.jpg',N'Thái Lan','LSP05'
EXEC sp_AddSP N'Quần short 4',660000,'24.jpg',N'Việt Nam','LSP05'
EXEC sp_AddSP N'Quần short 5',230000,'25.jpg',N'Hàn Quốc','LSP05'
--quần ống loe
EXEC sp_AddSP N'Quần ống loe 1',550000,'26.jpg',N'Trung Quốc','LSP06'
EXEC sp_AddSP N'Quần ống loe 2',720000,'27.jpg',N'Hàn Quốc','LSP06'
EXEC sp_AddSP N'Quần ống loe 3',190000,'28.jpg',N'Anh','LSP06'
EXEC sp_AddSP N'Quần ống loe 4',450000,'29.jpg',N'Việt Nam','LSP06'
EXEC sp_AddSP N'Quần ống loe 5',490000,'30.jpg',N'Mỹ','LSP06'
--quần skinny
EXEC sp_AddSP N'Quần skinny 1',170000,'31.jpg',N'Nhật','LSP07'
EXEC sp_AddSP N'Quần skinny 2',340000,'32.jpg',N'Hàn Quốc','LSP07'
EXEC sp_AddSP N'Quần skinny 3',640000,'33.jpg',N'Việt Nam','LSP07'
EXEC sp_AddSP N'Quần skinny 4',760000,'34.jpg',N'Thái Lan','LSP07'
EXEC sp_AddSP N'Quần skinny 5',380000,'35.jpg',N'Mỹ','LSP07'
--quẩn giả váy
EXEC sp_AddSP N'Quần giả váy 1',240000,'36.jpg',N'Hàn Quốc','LSP08'
EXEC sp_AddSP N'Quần giả váy 2',430000,'37.jpg',N'Nhật','LSP08'
EXEC sp_AddSP N'Quần giả váy 3',360000,'38.jpg',N'Thái Lan','LSP08'
EXEC sp_AddSP N'Quần giả váy 4',530000,'39.jpg',N'Việt Nam','LSP08'
EXEC sp_AddSP N'Quần giả váy 5',250000,'40.jpg',N'Hàn Quốc','LSP08'

--VÁY
--váy tennis
EXEC sp_AddSP N'Váy tennis 1',530000,'41.jpg',N'Việt Nam','LSP09'
EXEC sp_AddSP N'Váy tennis 2',240000,'42.jpg',N'Việt Nam','LSP09'
EXEC sp_AddSP N'Váy tennis 3',260000,'43.jpg',N'Nhật','LSP09'
EXEC sp_AddSP N'Váy tennis 4',240000,'44.jpg',N'Mỹ','LSP09'
EXEC sp_AddSP N'Váy tennis 5',320000,'45.jpg',N'Thái Lan','LSP09'
--váy chữ A
EXEC sp_AddSP N'Váy chữ A 1',360000,'46.jpg',N'Việt Nam','LSP10'
EXEC sp_AddSP N'Váy chữ A 2',420000,'47.jpg',N'Hàn Quốc','LSP10'
EXEC sp_AddSP N'Váy chữ A 3',330000,'48.jpg',N'Việt Nam','LSP10'
EXEC sp_AddSP N'Váy chữ A 4',660000,'49.jpg',N'Hàn Quốc','LSP10'
EXEC sp_AddSP N'Váy chữ A 5',200000,'50.jpg',N'Thái Lan','LSP10'
--váy jeans
EXEC sp_AddSP N'Váy jeans 1',240000,'51.jpg',N'Việt Nam','LSP11'
EXEC sp_AddSP N'Váy jeans 2',440000,'52.jpg',N'Việt Nam','LSP11'
EXEC sp_AddSP N'Váy jeans 3',540000,'53.jpg',N'Thái Lan','LSP11'
EXEC sp_AddSP N'Váy jeans 4',250000,'54.jpg',N'Nhật','LSP11'
EXEC sp_AddSP N'Váy jeans 5',540000,'55.jpg',N'Việt Nam','LSP11'
--váy xòe
EXEC sp_AddSP N'Váy xòe 1',430000,'56.jpg',N'Mỹ','LSP12'
EXEC sp_AddSP N'Váy xòe 2',370000,'57.jpg',N'Thái Lan','LSP12'
EXEC sp_AddSP N'Váy xòe 3',360000,'58.jpg',N'Hàn Quốc','LSP12'
EXEC sp_AddSP N'Váy xòe 4',370000,'59.jpg',N'Việt Nam','LSP12'
EXEC sp_AddSP N'Váy xòe 5',270000,'60.jpg',N'Việt Nam','LSP12'

--ĐẦM
--BODY
EXEC sp_AddSP N'Đầm body 1',760000,'61.jpg',N'Mỹ','LSP13'
EXEC sp_AddSP N'Đầm body 2',650000,'62.jpg',N'Hàn Quốc','LSP13'
EXEC sp_AddSP N'Đầm body 3',750000,'63.jpg',N'Nhật','LSP13'
EXEC sp_AddSP N'Đầm body 4',750000,'64.jpg',N'Nhật','LSP13'
EXEC sp_AddSP N'Đầm body 5',460000,'65.jpg',N'Việt Nam','LSP13'

--baby doll
EXEC sp_AddSP N'Đầm baby doll 1',750000,'66.jpg',N'Thái Lan','LSP14'
EXEC sp_AddSP N'Đầm baby doll 2',360000,'67.jpg',N'Mỹ','LSP14'
EXEC sp_AddSP N'Đầm baby doll 3',360000,'68.jpg',N'Hàn Quốc','LSP14'
EXEC sp_AddSP N'Đầm baby doll 4',220000,'69.jpg',N'Việt Nam','LSP14'
EXEC sp_AddSP N'Đầm baby doll 5',730000,'70.jpg',N'Trung Quốc','LSP14'

--đầm trễ vai
EXEC sp_AddSP N'Đầm trễ vai 1',360000,'71.jpg',N'Hàn Quốc','LSP15'
EXEC sp_AddSP N'Đầm trễ vai 2',740000,'72.jpg',N'Nhật','LSP15'
EXEC sp_AddSP N'Đầm trễ vai 3',460000,'73.jpg',N'Thái Lan','LSP15'
EXEC sp_AddSP N'Đầm trễ vai 4',340000,'74.jpg',N'Hàn Quốc','LSP15'
EXEC sp_AddSP N'Đầm trễ vai 5',230000,'75.jpg',N'Mỹ','LSP15'
--set
EXEC sp_AddSP N'Set 1',600000,'76.jpg',N'Việt Nam','LSP16'
EXEC sp_AddSP N'Set 2',750000,'77.jpg',N'Trung Quốc','LSP16'
EXEC sp_AddSP N'Set 3',540000,'78.jpg',N'Hàn Quốc','LSP16'
EXEC sp_AddSP N'Set 4',350000,'79.jpg',N'Mỹ','LSP16'
EXEC sp_AddSP N'Set 5',460000,'80.jpg',N'Việt Nam','LSP16'

--BẢNG KHO

INSERT INTO KHO 
VALUES
-- ÁO TRỄ VAI
(1,'S',10),
(2,'S',30),
(3,'S',8),
(4,'S',30),
(5,'S',30),
--
(1,'M',30),
(2,'M',30),
(3,'M',30),
(4,'M',30),
(5,'M',30),
--
(1,'L',30),
(2,'L',30),
(3,'L',30),
(4,'L',7),
(5,'L',30),
--
(1,'XL',30),
(2,'XL',30),
(3,'XL',30),
(4,'XL',30),
(5,'XL',9),

--------------------------------

--ÁO CROP TOP
(6,'S',30),
(7,'S',30),
(8,'S',30),
(9,'S',30),
(10, 'S',8),

---------------
(6,'M',30),
(7,'M',30),
(8,'M',30),
(9,'M',30),
(10, 'M',30),
-------------
(6,'L',30),
(7,'L',30),
(8,'L',6),
(9,'L',30),
(10, 'L',30),
---------------
(6,'XL',30),
(7,'XL',30),
(8,'XL',30),
(9,'XL',5),
(10, 'XL',30),

--ÁO SƠ MI

(11, 'S',30),
(12, 'S',30),
(13, 'S',30),
(14, 'S',30),
(15, 'S',30),
------------------

(11, 'M',30),
(12, 'M',30),
(13, 'M',30),
(14, 'M',30),
(15, 'M',30),
-------------------

(11, 'L',30),
(12, 'L',30),
(13, 'L',30),
(14, 'L',6),
(15, 'L',30),
--

(11, 'XL',30),
(12, 'XL',30),
(13, 'XL',30),
(14, 'XL',7),
(15, 'XL',30),
-----------------
--ÁO HAI DÂY
(16, 'S',30),
(17, 'S',30),
(18, 'S',30),
(19, 'S',30),
(20, 'S',30),
--
(16, 'M',30),
(17, 'M',30),
(18, 'M',30),
(19, 'M',30),
(20, 'M',30),
--
(16, 'L',30),
(17, 'L',30),
(18, 'L',30),
(19, 'L',8),
(20, 'L',30),
--
(16, 'XL',30),
(17, 'XL',30),
(18, 'XL',30),
(19, 'XL',30),
(20, 'XL',30),
--QUẦN SHORT

(21, 'S',30),
(22, 'S',30),
(23, 'S',30),
(24, 'S',30),
(25, 'S',30),
--

(21, 'M',30),
(22, 'M',30),
(23, 'M',30),
(24, 'M',2),
(25, 'M',30),
--

(21, 'L',30),
(22, 'L',30),
(23, 'L',30),
(24, 'L',30),
(25, 'L',30),
--

(21, 'XL',30),
(22, 'XL',30),
(23, 'XL',30),
(24, 'XL',30),
(25, 'XL',30),
---
--QUẦN ỐNG LOE
--
(26, 'S',30),
(27, 'S',30),
(28, 'S',30),
(29, 'S',30),
(30, 'S',4),
--

(26, 'M',30),
(27, 'M',30),
(28, 'M',30),
(29, 'M',30),
(30, 'M',30),
--
(26, 'L',30),
(27, 'L',30),
(28, 'L',30),
(29, 'L',30),
(30, 'L',30),
--
(26, 'XL',30),
(27, 'XL',30),
(28, 'XL',30),
(29, 'XL',30),
(30, 'XL',30),
--QUẦN SKINNY

(31, 'S',30),
(32, 'S',30),
(33, 'S',30),
(34, 'S',30),
(35, 'S',30),
--

(31, 'M',30),
(32, 'M',30),
(33, 'M',30),
(34, 'M',30),
(35, 'M',30),
--


(31, 'L',30),
(32, 'L',30),
(33, 'L',6),
(34, 'L',30),
(35, 'L',30),
--


(31, 'XL',30),
(32, 'XL',30),
(33, 'XL',30),
(34, 'XL',9),
(35, 'XL',30),
---QUẦN GIẢ VÁY
--
(36, 'S',30),
(37, 'S',30),
(38, 'S',30),
(39, 'S',30),
(40, 'S',30),
--
(36, 'M',30),
(37, 'M',30),
(38, 'M',30),
(39, 'M',30),
(40, 'M',30),
--
(36, 'L',30),
(37, 'L',30),
(38, 'L',30),
(39, 'L',30),
(40, 'L',30),
--

(36, 'XL',30),
(37, 'XL',30),
(38, 'XL',8),
(39, 'XL',30),
(40, 'XL',30),
--VÁY TENNIS

(41, 'S',30),
(42, 'S',30),
(43, 'S',30),
(44, 'S',30),
(45, 'S',30),
--

(41, 'M',30),
(42, 'M',30),
(43, 'M',8),
(44,  'M',30),
(45,  'M',30),
--


(41, 'L',30),
(42, 'L',30),
(43, 'L',30),
(44,  'L',30),
(45,  'L',30),
--

(41, 'XL',30),
(42, 'XL',30),
(43, 'XL',6),
(44,  'XL',30),
(45,  'XL',30),
---
--VÁY CHỮ A

(46,  'S',30),
(47,  'S',30),
(48,  'S',30),
(49,  'S',30),
(50,  'S',30),
--

(46,  'M',30),
(47,  'M',30),
(48,  'M',30),
(49,  'M',30),
(50,  'M',30),
--

(46,  'L',30),
(47,  'L',30),
(48,  'L',30),
(49,  'L',30),
(50,  'L',30),
--

(46,  'XL',30),
(47,  'XL',30),
(48,  'XL',30),
(49,  'XL',30),
(50,  'XL',30),

--VÁY JEANS
(51,  'S',30),
(52,  'S',30),
(53,  'S',30),
(54,  'S',30),
(55,  'S',30),
--
(51,  'M',30),
(52,  'M',30),
(53,  'M',30),
(54,  'M',30),
(55,  'M',30),
--

(51,  'L',30),
(52,  'L',30),
(53,  'L',30),
(54,  'L',30),
(55,  'L',30),
--

(51,  'XL',30),
(52,  'XL',30),
(53,  'XL',30),
(54,  'XL',30),
(55,  'XL',30),

--VÁY XÒE
(56,  'S',30),
(57,  'S',30),
(58,  'S',30),
(59,  'S',30),
(60,  'S',30),
--
(56,  'M',30),
(57,  'M',30),
(58,  'M',30),
(59,  'M',30),
(60,  'M',30),
--
(56,  'L',30),
(57,  'L',30),
(58,  'L',30),
(59,  'L',30),
(60,  'L',30),
--
(56,  'XL',30),
(57,  'XL',30),
(58,  'XL',30),
(59,  'XL',30),
(60,  'XL',30),


--ĐẦM BODY
(61,  'S',30),
(62,  'S',30),
(63,  'S',30),
(64,  'S',30),
(65,  'S',30),
--
(61,  'M',30),
(62,  'M',30),
(63,  'M',30),
(64,  'M',30),
(65,  'M',30),
--
(61,  'L',30),
(62,  'L',30),
(63,  'L',30),
(64,  'L',30),
(65,  'L',30),
--
(61,  'XL',30),
(62,  'XL',30),
(63,  'XL',30),
(64,  'XL',30),
(65,  'XL',30),

--ĐẦM BABY DOLL
(66,  'S',30),
(67,  'S',30),
(68,  'S',30),
(69,  'S',30),
(70,  'S',30),
--
(66,  'M',30),
(67,  'M',30),
(68,  'M',30),
(69,  'M',30),
(70,  'M',30),
--
(66,  'L',30),
(67,  'L',30),
(68,  'L',30),
(69,  'L',30),
(70,  'L',30),
--
(66,  'XL',30),
(67,  'XL',30),
(68,  'XL',30),
(69,  'XL',30),
(70,  'XL',30),


--ĐẦM TRỄ VAI

(71,  'S',30),
(72,  'S',30),
(73,  'S',30),
(74,  'S',30),
(75,  'S',30),
--
(71,  'M',30),
(72,  'M',30),
(73,  'M',30),
(74,  'M',30),
(75,  'M',30),
--
(71,  'L',30),
(72,  'L',30),
(73,  'L',30),
(74,  'L',30),
(75,  'L',30),
--
(71,  'XL',30),
(72,  'XL',30),
(73,  'XL',30),
(74,  'XL',30),
(75,  'XL',30),
--SET
(76,  'S',30),
(77,  'S',30),
(78,  'S',30),
(79,  'S',30),
(80,  'S',30),
--
(76,  'M',30),
(77,  'M',30),
(78,  'M',30),
(79,  'M',30),
(80,  'M',30),
--
(76,  'L',30),
(77,  'L',30),
(78,  'L',30),
(79,  'L',30),
(80,  'L',30),
--
(76,  'XL',30),
(77,  'XL',30),
(78,  'XL',30),
(79,  'XL',30),
(80,  'XL',30)

select TENSP,TENLOAI from SANPHAM,LOAISP WHERE SANPHAM.MALSP=LOAISP.MALSP

--BẢNG HÓA ĐƠN
	CREATE PROC sp_AddHD(
    @NGTAO DATETIME, -- NGÀY TẠO HÓA ĐƠN
    @THANHTOAN FLOAT, -- TỔNG (SỐ LƯỢNG * ĐƠN GIÁ)
    @TINHTRANG NVARCHAR(50), -- CHƯA THANHTOAN,DATT
    @MAKH VARCHAR(11),
    @MANV VARCHAR(10) )

AS 
	--THÊM MỚI DỮ LIỆU
	INSERT INTO HOADON(NGTAO,THANHTOAN,TINHTRANG,MAKH,MANV)
	VALUES(@NGTAO,@THANHTOAN,@TINHTRANG,@MAKH,@MANV)
GO

EXEC sp_AddHD '08/12/2020','1',N'Đã thanh toán','0934214565','NV01'
EXEC sp_AddHD '08/01/2020','1',N'Đã thanh toán','0948571923','NV02'
EXEC sp_AddHD '08/05/2020','1',N'Đã thanh toán','0968492918','NV03'
EXEC sp_AddHD '08/05/2022','1',N'Đã thanh toán','0968492918','NV01'

--BẢNG CHI TIẾT HÓA ĐƠN
CREATE PROC sp_AddCTHD(
    @MAHD INT,
    @MASP INT,
	@SIZE VARCHAR(10),
    @SOLUONG INT)

AS 
	--KIỂM TRA TRÙNG KHÓA CHÍNH
	IF EXISTS(SELECT * FROM CHITIETHD WHERE MASP=@MASP AND MAHD=@MAHD AND SIZE=@SIZE)
		RETURN 0
	--THÊM MỚI DỮ LIỆU
	INSERT INTO CHITIETHD(MAHD,MASP,SIZE,SOLUONG)
	VALUES(@MAHD,@MASP,@SIZE,@SOLUONG)
GO

EXEC sp_AddCTHD '1','6','L','1'
EXEC sp_AddCTHD '1','23','M','1'
EXEC sp_AddCTHD '1','7','S','1'
EXEC sp_AddCTHD '1','32','M','1'
EXEC sp_AddCTHD '1','40','XL','1'
EXEC sp_AddCTHD '1','69','M','1'

EXEC sp_AddCTHD '2','4','M','1'
EXEC sp_AddCTHD '2','11','XL','1'
EXEC sp_AddCTHD '2','36','L','1'
EXEC sp_AddCTHD '2','1','S','1'
EXEC sp_AddCTHD '2','2','L','1'
EXEC sp_AddCTHD '2','3','M','1'


EXEC sp_AddCTHD '3','42','S','1'
EXEC sp_AddCTHD '3','67','L','1'
EXEC sp_AddCTHD '3','8','S','1'
EXEC sp_AddCTHD '3','9','XL','1'
EXEC sp_AddCTHD '3','3','M','1'
EXEC sp_AddCTHD '3','5','L','1'
EXEC sp_AddCTHD '3','67','M','2'

EXEC sp_AddCTHD '4','66','M','2'
EXEC sp_AddCTHD '4','66','L','2'

SELECT * FROM CHITIETHD
SELECT * FROM HOADON
SELECT * FROM SANPHAM

--Thống kê doanh thu
CREATE PROC up_RevenueStatistics @From Date, @To Date
AS
	SELECT CONVERT(DATE,NGTAO) 'NgayXuatHD', SUM(THANHTOAN) 'ThanhTien'
	FROM HOADON 
	WHERE CONVERT(DATE,NGTAO) BETWEEN @From AND @To 
	GROUP BY CONVERT(DATE,NGTAO)
GO

--Thống kê sản phẩm
CREATE PROC up_ProductStatistics @From Date, @To Date
AS
	SELECT TENSP, SUM(CHITIETHD.SOLUONG) 'TongSoLuong'
	FROM HOADON, CHITIETHD, SANPHAM
	WHERE HOADON.MAHD=CHITIETHD.MAHD AND CHITIETHD.MASP=SANPHAM.MASP
	AND CONVERT(DATE, NGTAO) BETWEEN @From AND @To
	GROUP BY SANPHAM.MASP, TENSP
GO


CREATE PROC sp_ChartSanPham
@nam int
AS
	SELECT TOP 10 TENSP, SUM(CTHD.SOLUONG) SOLUONGBANRA
	FROM HOADON HD JOIN CHITIETHD CTHD 
		ON HD.MAHD = CTHD.MAHD JOIN SANPHAM SP
		ON SP.MASP = CTHD.MASP
	WHERE YEAR(NGTAO) = @nam
	GROUP BY TENSP
	ORDER BY SOLUONGBANRA DESC
GO

CREATE PROC sp_ChartNhanVien
@nam int
AS
	SELECT HOTEN, SUM(THANHTOAN) DOANHTHU
	FROM HOADON HD JOIN NHANVIEN NV
		ON NV.MANV = HD.MANV JOIN THONGTINTAIKHOAN TTTK
		ON TTTK.MATK = NV.MATK
	WHERE YEAR(HD.NGTAO) = @nam
	GROUP BY HOTEN
	ORDER BY DOANHTHU DESC
GO

CREATE PROC sp_ChartDoanhThu
@nam int
AS
	DECLARE @tbDoanhThu TABLE (THANG INT, DOANHTHU FLOAT)
	INSERT @tbDoanhThu
		SELECT m, SUM(THANHTOAN) DOANHTHU
		FROM (SELECT * FROM HOADON WHERE YEAR(NGTAO) = @nam) dtn RIGHT JOIN (
			SELECT m
			FROM (VALUES (1), (2), (3), (4), (5), (6), (7), (8), (9), (10), (11), (12))
			[1 to 12](m)
		) listMonth
			ON MONTH(NGTAO)=listMonth.m
		GROUP BY m
		ORDER BY m
	
	UPDATE @tbDoanhThu SET DOANHTHU = 0 WHERE DOANHTHU IS NULL
	SELECT * FROM @tbDoanhThu
GO

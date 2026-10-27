USE master;
GO

ALTER DATABASE QuanLySuaXe
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;
GO

DROP DATABASE QuanLySuaXe;
GO

IF DB_ID(N'QuanLySuaXe') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLySuaXe SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLySuaXe;
END;
GO

CREATE DATABASE QuanLySuaXe;
GO

USE QuanLySuaXe;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF EXISTS (
        SELECT 1
        FROM sys.tables
        WHERE schema_id = SCHEMA_ID(N'dbo')
          AND name IN (
              N'ChucVu', N'NhanVien', N'KhachHang', N'Xe',
              N'NhaCungCap', N'PhuTung', N'DichVu',
              N'PhieuTiepNhan', N'ChiTietPhieuTiepNhan',
              N'PhanCongTho', N'HoaDon', N'ChiTietHoaDon',
              N'PhieuNhap', N'ChiTietPhieuNhap'
          )
    )
        THROW 50001,
              N'Database đã có bảng cũ. Script này dùng để tạo mới trên database trống.',
              1;

    -- =================================================
    -- 1. CHỨC VỤ
    -- =================================================
    CREATE TABLE dbo.ChucVu (
        MaChucVu VARCHAR(10) NOT NULL PRIMARY KEY,
        TenChucVu NVARCHAR(50) NOT NULL,
        MoTa NVARCHAR(200) NOT NULL
    );

    -- =================================================
    -- 2. NHÂN VIÊN / TÀI KHOẢN
    -- =================================================
    CREATE TABLE dbo.NhanVien (
        MaNhanVien VARCHAR(10) NOT NULL PRIMARY KEY,
        MaChucVu VARCHAR(10) NOT NULL,
        TenNhanVien NVARCHAR(100) NOT NULL,
        TenDangNhap VARCHAR(50) NOT NULL UNIQUE,
        MatKhauHash VARCHAR(255) NOT NULL,
        SoDienThoai VARCHAR(15) NOT NULL,
        TrangThai BIT NOT NULL DEFAULT 1,
        SoXeDangSuaToiDa INT NOT NULL DEFAULT 3,

        CONSTRAINT FK_NhanVien_ChucVu
            FOREIGN KEY (MaChucVu)
            REFERENCES dbo.ChucVu(MaChucVu),

        CONSTRAINT CK_NhanVien_SoXe
            CHECK (SoXeDangSuaToiDa >= 0)
    );

    -- =================================================
    -- 3. KHÁCH HÀNG
    -- =================================================
    CREATE TABLE dbo.KhachHang (
        MaKhachHang VARCHAR(10) NOT NULL PRIMARY KEY,
        TenKhachHang NVARCHAR(100) NOT NULL,
        SoDienThoai VARCHAR(15) NOT NULL,
        DiaChi NVARCHAR(200) NOT NULL
    );

    -- =================================================
    -- 4. XE
    -- =================================================
    CREATE TABLE dbo.Xe (
        MaXe VARCHAR(10) NOT NULL PRIMARY KEY,
        MaKhachHang VARCHAR(10) NOT NULL,
        BienSo VARCHAR(15) NOT NULL UNIQUE,
        HangXe NVARCHAR(50) NOT NULL,
        DongXe NVARCHAR(50) NOT NULL,
        NamSanXuat INT NOT NULL,
        MauSac NVARCHAR(30) NOT NULL,
        SoKhung VARCHAR(50) NOT NULL,

        CONSTRAINT FK_Xe_KhachHang
            FOREIGN KEY (MaKhachHang)
            REFERENCES dbo.KhachHang(MaKhachHang),

        CONSTRAINT UQ_Xe_KhachHang
            UNIQUE (MaXe, MaKhachHang)
    );

    -- =================================================
    -- 5. NHÀ CUNG CẤP
    -- =================================================
    CREATE TABLE dbo.NhaCungCap (
        MaNCC VARCHAR(10) NOT NULL PRIMARY KEY,
        TenNCC NVARCHAR(100) NOT NULL,
        SoDienThoai VARCHAR(15) NOT NULL,
        DiaChi NVARCHAR(200) NOT NULL
    );

    -- =================================================
    -- 6. PHỤ TÙNG
    -- =================================================
    CREATE TABLE dbo.PhuTung (
        MaPhuTung VARCHAR(10) NOT NULL PRIMARY KEY,
        TenPhuTung NVARCHAR(100) NOT NULL,
        MaNCC VARCHAR(10) NOT NULL,
        HanSuDung DATE NOT NULL,
        DonGiaNhap DECIMAL(18,2) NOT NULL DEFAULT 0,
        DonGiaBan DECIMAL(18,2) NOT NULL DEFAULT 0,
        SoLuongTon INT NOT NULL DEFAULT 0,
        TrangThai BIT NOT NULL DEFAULT 1,

        CONSTRAINT FK_PhuTung_NhaCungCap
            FOREIGN KEY (MaNCC)
            REFERENCES dbo.NhaCungCap(MaNCC),

        CONSTRAINT CK_PhuTung_Gia
            CHECK (DonGiaNhap >= 0 AND DonGiaBan >= 0),

        CONSTRAINT CK_PhuTung_SoLuong
            CHECK (SoLuongTon >= 0)
    );

    -- =================================================
    -- 7. DỊCH VỤ
    -- =================================================
    CREATE TABLE dbo.DichVu (
        MaDichVu VARCHAR(10) NOT NULL PRIMARY KEY,
        TenDichVu NVARCHAR(100) NOT NULL,
        MoTa NVARCHAR(300) NOT NULL,
        DonGiaDichVu DECIMAL(18,2) NOT NULL DEFAULT 0,
        ThoiGianDuKien INT NOT NULL,
        MaPhuTung VARCHAR(10) NOT NULL,
        TrangThai BIT NOT NULL DEFAULT 1,

        CONSTRAINT FK_DichVu_PhuTung
            FOREIGN KEY (MaPhuTung)
            REFERENCES dbo.PhuTung(MaPhuTung),

        CONSTRAINT CK_DichVu_Gia
            CHECK (DonGiaDichVu >= 0),

        CONSTRAINT CK_DichVu_ThoiGian
            CHECK (ThoiGianDuKien >= 0)
    );

    -- =================================================
    -- 8. PHIẾU TIẾP NHẬN
    -- =================================================
    CREATE TABLE dbo.PhieuTiepNhan (
        MaPhieuTiepNhan VARCHAR(10) NOT NULL PRIMARY KEY,
        MaKhachHang VARCHAR(10) NOT NULL,
        MaXe VARCHAR(10) NOT NULL,
        MaNhanVien VARCHAR(10) NOT NULL,
        ThoiGianNhan DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        ThoiGianHoanThanh DATETIME2 NOT NULL,
        MoTaTinhTrangXe NVARCHAR(500) NOT NULL,
        TrangThai NVARCHAR(30) NOT NULL,
        GhiChu NVARCHAR(300) NOT NULL,

        CONSTRAINT FK_PhieuTN_KhachHang
            FOREIGN KEY (MaKhachHang)
            REFERENCES dbo.KhachHang(MaKhachHang),

        CONSTRAINT FK_PhieuTN_Xe
            FOREIGN KEY (MaXe, MaKhachHang)
            REFERENCES dbo.Xe(MaXe, MaKhachHang),

        CONSTRAINT FK_PhieuTN_NhanVien
            FOREIGN KEY (MaNhanVien)
            REFERENCES dbo.NhanVien(MaNhanVien),

        CONSTRAINT CK_PhieuTN_ThoiGian
            CHECK (ThoiGianHoanThanh >= ThoiGianNhan)
    );

    -- =================================================
    -- 9. CHI TIẾT PHIẾU TIẾP NHẬN
    -- =================================================
    CREATE TABLE dbo.ChiTietPhieuTiepNhan (
        MaChiTietTiepNhan INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        MaPhieuTiepNhan VARCHAR(10) NOT NULL,
        MaDichVu VARCHAR(10) NOT NULL,
        SoLuong INT NOT NULL DEFAULT 1,
        DonGiaDichVu DECIMAL(18,2) NOT NULL,

        CONSTRAINT FK_CTTN_PhieuTN
            FOREIGN KEY (MaPhieuTiepNhan)
            REFERENCES dbo.PhieuTiepNhan(MaPhieuTiepNhan),

        CONSTRAINT FK_CTTN_DichVu
            FOREIGN KEY (MaDichVu)
            REFERENCES dbo.DichVu(MaDichVu),

        CONSTRAINT CK_CTTN_SoLuong
            CHECK (SoLuong > 0),

        CONSTRAINT CK_CTTN_DonGia
            CHECK (DonGiaDichVu >= 0),

        CONSTRAINT UQ_CTTN_Phieu_DichVu
            UNIQUE (MaPhieuTiepNhan, MaDichVu)
    );

    -- =================================================
    -- 10. PHÂN CÔNG THỢ
    -- =================================================
    CREATE TABLE dbo.PhanCongTho (
        MaPhieuTiepNhan VARCHAR(10) NOT NULL,
        MaNhanVien VARCHAR(10) NOT NULL,
        VaiTro NVARCHAR(50) NOT NULL,
        TrangThaiCongViec NVARCHAR(30) NOT NULL,
        ThoiGianBatDau DATETIME2 NOT NULL,
        ThoiGianKetThuc DATETIME2 NOT NULL,

        CONSTRAINT PK_PhanCongTho
            PRIMARY KEY (MaPhieuTiepNhan, MaNhanVien),

        CONSTRAINT FK_PhanCong_PhieuTN
            FOREIGN KEY (MaPhieuTiepNhan)
            REFERENCES dbo.PhieuTiepNhan(MaPhieuTiepNhan),

        CONSTRAINT FK_PhanCong_NhanVien
            FOREIGN KEY (MaNhanVien)
            REFERENCES dbo.NhanVien(MaNhanVien),

        CONSTRAINT CK_PhanCong_ThoiGian
            CHECK (ThoiGianKetThuc >= ThoiGianBatDau)
    );

    -- =================================================
    -- 11. HÓA ĐƠN
    -- =================================================
    CREATE TABLE dbo.HoaDon (
        MaHoaDon VARCHAR(10) NOT NULL PRIMARY KEY,
        MaPhieuTiepNhan VARCHAR(10) NOT NULL UNIQUE,
        NgayLap DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        TongTienDichVu DECIMAL(18,2) NOT NULL DEFAULT 0,
        TongTienPhuTung DECIMAL(18,2) NOT NULL DEFAULT 0,
        TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
        TrangThaiThanhToan NVARCHAR(30) NOT NULL,
        PhuongThucThanhToan NVARCHAR(30) NOT NULL,
        MaNhanVien VARCHAR(10) NOT NULL,

        CONSTRAINT FK_HoaDon_PhieuTN
            FOREIGN KEY (MaPhieuTiepNhan)
            REFERENCES dbo.PhieuTiepNhan(MaPhieuTiepNhan),

        CONSTRAINT FK_HoaDon_NhanVien
            FOREIGN KEY (MaNhanVien)
            REFERENCES dbo.NhanVien(MaNhanVien),

        CONSTRAINT CK_HoaDon_Tien
            CHECK (
                TongTienDichVu >= 0
                AND TongTienPhuTung >= 0
                AND TongTien >= 0
                AND TongTien = TongTienDichVu + TongTienPhuTung
            )
    );

    -- =================================================
    -- 12. CHI TIẾT HÓA ĐƠN
    -- =================================================
    CREATE TABLE dbo.ChiTietHoaDon (
        MaChiTietHoaDon INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        MaHoaDon VARCHAR(10) NOT NULL,
        MaDichVu VARCHAR(10) NOT NULL,
        TenDichVu NVARCHAR(100) NOT NULL,
        SoLuong INT NOT NULL,
        DonGia DECIMAL(18,2) NOT NULL,

        ThanhTien AS (SoLuong * DonGia) PERSISTED NOT NULL,

        CONSTRAINT FK_CTHD_HoaDon
            FOREIGN KEY (MaHoaDon)
            REFERENCES dbo.HoaDon(MaHoaDon),

        CONSTRAINT FK_CTHD_DichVu
            FOREIGN KEY (MaDichVu)
            REFERENCES dbo.DichVu(MaDichVu),

        CONSTRAINT CK_CTHD_SoLuong
            CHECK (SoLuong > 0),

        CONSTRAINT CK_CTHD_DonGia
            CHECK (DonGia >= 0)
    );

    -- =================================================
    -- 13. PHIẾU NHẬP
    -- =================================================
    CREATE TABLE dbo.PhieuNhap (
        MaPhieuNhap VARCHAR(10) NOT NULL PRIMARY KEY,
        NgayNhap DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        MaNhanVienNhap VARCHAR(10) NOT NULL,
        GhiChu NVARCHAR(300) NOT NULL,
        TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,

        CONSTRAINT FK_PhieuNhap_NhanVien
            FOREIGN KEY (MaNhanVienNhap)
            REFERENCES dbo.NhanVien(MaNhanVien),

        CONSTRAINT CK_PhieuNhap_TongTien
            CHECK (TongTien >= 0)
    );

    -- =================================================
    -- 14. CHI TIẾT PHIẾU NHẬP
    -- =================================================
    CREATE TABLE dbo.ChiTietPhieuNhap (
        MaChiTietPhieuNhap INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        MaPhieuNhap VARCHAR(10) NOT NULL,
        MaPhuTung VARCHAR(10) NOT NULL,
        SoLuong INT NOT NULL,
        DonGiaNhap DECIMAL(18,2) NOT NULL,

        ThanhTien AS (SoLuong * DonGiaNhap) PERSISTED NOT NULL,

        CONSTRAINT FK_CTPNhap_PhieuNhap
            FOREIGN KEY (MaPhieuNhap)
            REFERENCES dbo.PhieuNhap(MaPhieuNhap),

        CONSTRAINT FK_CTPNhap_PhuTung
            FOREIGN KEY (MaPhuTung)
            REFERENCES dbo.PhuTung(MaPhuTung),


        CONSTRAINT CK_CTPNhap_SoLuong
            CHECK (SoLuong > 0),

        CONSTRAINT CK_CTPNhap_DonGia
            CHECK (DonGiaNhap >= 0)
    );

    -- =================================================
    -- DỮ LIỆU MẪU
    -- Tất cả giá trị đều được điền đầy đủ.
    -- Các phiếu sửa chữa mẫu đều đã hoàn thành.
    -- Hạn sử dụng 9999-12-31: phụ tùng không có hạn dùng.
    -- =================================================

    INSERT INTO dbo.ChucVu (MaChucVu, TenChucVu, MoTa)
    VALUES
        ('CV001', N'Chủ cửa hàng', N'Quản lý toàn bộ cửa hàng'),
        ('CV002', N'Thu ngân', N'Tiếp nhận xe và thanh toán'),
        ('CV003', N'Kho', N'Quản lý nhập hàng và tồn kho'),
        ('CV004', N'Thợ', N'Kiểm tra và sửa chữa xe');

    -- Mật khẩu chung của tài khoản mẫu: Demo1234.
    -- Hash PBKDF2-SHA256, 600000 vòng, mỗi tài khoản có salt riêng.
    INSERT INTO dbo.NhanVien (
        MaNhanVien, MaChucVu, TenNhanVien, TenDangNhap,
        MatKhauHash, SoDienThoai, TrangThai, SoXeDangSuaToiDa
    )
    VALUES
        ('NV001', 'CV001', N'Nguyễn Quang Minh', 'mau_admin',
         'PBKDF2-SHA256$600000$WaWcJdk5KD+cN9pnhUM65A==$xrykyzU1l6QixzmDdAKlWZazfHNdXydzoRCsg2OSZJk=',
         '0900000001', 1, 0),

        ('NV002', 'CV002', N'Trần Thu Hà', 'mau_thungan',
         'PBKDF2-SHA256$600000$nfyi1c/5cQuWx8fj9pfNRQ==$Y8WAL+Htw2PMuHeEITSMKQ9ljmeXNsAdtG8R5HA/y00=',
         '0900000002', 1, 0),

        ('NV003', 'CV003', N'Lê Đức Huy', 'mau_kho',
         'PBKDF2-SHA256$600000$8B4rqvCGNKa11qfP+3G3Jg==$EN11ho7vf/miQsqNYMbIRmcKdU8nMHToK1WF7rHLkoE=',
         '0900000003', 1, 0),

        ('NV004', 'CV004', N'Phạm Văn Nam', 'mau_tho',
         'PBKDF2-SHA256$600000$Y/aq3qLMD0nJEapiy924+g==$ex9UJQ7dw/fPKb9SpBRqItZhbguiJTNd8GX3MeEjUIU=',
         '0900000004', 1, 3);

    INSERT INTO dbo.KhachHang (
        MaKhachHang, TenKhachHang, SoDienThoai, DiaChi
    )
    VALUES
        ('KH001', N'Nguyễn Hải An', '0910000001', N'Số 10 đường Mẫu, Hà Nội'),
        ('KH002', N'Trần Ngọc Mai', '0910000002', N'Số 20 đường Mẫu, Hà Nội'),
        ('KH003', N'Lê Hoàng Phúc', '0910000003', N'Số 30 đường Mẫu, Hà Nội');

    INSERT INTO dbo.Xe (
        MaXe, MaKhachHang, BienSo, HangXe,
        DongXe, NamSanXuat, MauSac, SoKhung
    )
    VALUES
        ('XE001', 'KH001', '29A1-990.01', N'Honda',
         N'Wave Alpha', 2021, N'Đỏ', 'MAU-SOKHUNG-001'),

        ('XE002', 'KH002', '29B1-990.02', N'Honda',
         N'Vision', 2022, N'Trắng', 'MAU-SOKHUNG-002'),

        ('XE003', 'KH003', '29C1-990.03', N'Yamaha',
         N'Exciter', 2020, N'Xanh', 'MAU-SOKHUNG-003');

    INSERT INTO dbo.NhaCungCap (
        MaNCC, TenNCC, SoDienThoai, DiaChi
    )
    VALUES
        ('NCC001', N'Dầu nhớt Minh Phát (mẫu)',
         '0920000001', N'Số 40 đường Mẫu, Hà Nội'),

        ('NCC002', N'Phụ tùng An Bình (mẫu)',
         '0920000002', N'Số 50 đường Mẫu, Hà Nội'),

        ('NCC003', N'Điện xe Thành Nam (mẫu)',
         '0920000003', N'Số 60 đường Mẫu, Hà Nội');

    -- Tồn kho đã trừ các phụ tùng dùng cho ba phiếu hoàn thành.
    INSERT INTO dbo.PhuTung (
        MaPhuTung, TenPhuTung, MaNCC, HanSuDung,
        DonGiaNhap, DonGiaBan, SoLuongTon, TrangThai
    )
    VALUES
        ('PT001', N'Dầu nhớt 0,8 lít', 'NCC001', '20281001',
         80000, 100000, 0, 1),
        ('PT002', N'Bugi xe máy', 'NCC002', '99991231',
         25000, 40000, 0, 1),
        ('PT003', N'Má phanh trước', 'NCC002', '99991231',
         60000, 90000, 0, 1),
        ('PT004', N'Lọc gió xe tay ga', 'NCC002', '99991231',
         35000, 50000, 0, 1),
        ('PT005', N'Ắc quy xe máy 12V', 'NCC003', '20281010',
         280000, 350000, 0, 1),
        ('PT006', N'Dây curoa xe tay ga', 'NCC002', '99991231',
         150000, 200000, 0, 1);

    -- Đơn giá dịch vụ chỉ là tiền công, chưa gồm phụ tùng.
    INSERT INTO dbo.DichVu (
        MaDichVu, TenDichVu, MoTa, DonGiaDichVu,
        ThoiGianDuKien, MaPhuTung, TrangThai
    )
    VALUES
        ('DV001', N'Thay dầu máy', N'Thay dầu và kiểm tra mức dầu',
         30000, 15, 'PT001', 1),

        ('DV002', N'Thay bugi', N'Thay bugi và kiểm tra đánh lửa',
         20000, 15, 'PT002', 1),

        ('DV003', N'Thay má phanh trước', N'Thay má phanh và căn chỉnh',
         40000, 30, 'PT003', 1),

        ('DV004', N'Thay lọc gió', N'Thay lọc gió và vệ sinh hộp lọc',
         20000, 20, 'PT004', 1),

        ('DV005', N'Thay ắc quy', N'Lắp ắc quy và kiểm tra điện sạc',
         30000, 20, 'PT005', 1),

        ('DV006', N'Thay dây curoa', N'Thay dây curoa và vệ sinh truyền động',
         60000, 45, 'PT006', 1);

    INSERT INTO dbo.PhieuTiepNhan (
        MaPhieuTiepNhan, MaKhachHang, MaXe, MaNhanVien,
        ThoiGianNhan, ThoiGianHoanThanh,
        MoTaTinhTrangXe, TrangThai, GhiChu
    )
    VALUES
        ('TN001', 'KH001', 'XE001', 'NV002',
         '2026-10-20T08:00:00', '2026-10-20T09:00:00',
         N'Xe khó nổ, cần thay dầu và bugi',
         N'Đã hoàn thành', N'Đã bàn giao và thanh toán'),

        ('TN002', 'KH002', 'XE002', 'NV002',
         '2026-10-21T09:00:00', '2026-10-21T10:10:00',
         N'Phanh trước kêu, lọc gió bẩn',
         N'Đã hoàn thành', N'Đã chạy thử và bàn giao xe'),

        ('TN003', 'KH003', 'XE003', 'NV002',
         '2026-10-22T14:00:00', '2026-10-22T15:05:00',
         N'Ắc quy yếu, xe không đề được',
         N'Đã hoàn thành', N'Đã sửa xong, chờ thanh toán');

    INSERT INTO dbo.ChiTietPhieuTiepNhan (
        MaPhieuTiepNhan, MaDichVu, SoLuong, DonGiaDichVu
    )
    VALUES
        ('TN001', 'DV001', 1, 30000),
        ('TN001', 'DV002', 1, 20000),
        ('TN002', 'DV003', 1, 40000),
        ('TN002', 'DV004', 1, 20000),
        ('TN003', 'DV005', 1, 30000);

    INSERT INTO dbo.PhanCongTho (
        MaPhieuTiepNhan, MaNhanVien, VaiTro,
        TrangThaiCongViec, ThoiGianBatDau, ThoiGianKetThuc
    )
    VALUES
        ('TN001', 'NV004', N'Thợ chính', N'Đã hoàn thành',
         '2026-10-20T08:10:00', '2026-10-20T08:55:00'),

        ('TN002', 'NV004', N'Thợ chính', N'Đã hoàn thành',
         '2026-10-21T09:10:00', '2026-10-21T10:05:00'),

        ('TN003', 'NV004', N'Thợ chính', N'Đã hoàn thành',
         '2026-10-22T14:10:00', '2026-10-22T15:00:00');

    INSERT INTO dbo.HoaDon (
        MaHoaDon, MaPhieuTiepNhan, NgayLap,
        TongTienDichVu, TongTienPhuTung, TongTien,
        TrangThaiThanhToan, PhuongThucThanhToan, MaNhanVien
    )
    VALUES
        ('HD001', 'TN001', '2026-10-20T09:05:00',
         50000, 140000, 190000,
         N'Đã thanh toán', N'Tiền mặt', 'NV002'),

        ('HD002', 'TN002', '2026-10-21T10:15:00',
         60000, 140000, 200000,
         N'Đã thanh toán', N'Chuyển khoản', 'NV002'),

        ('HD003', 'TN003', '2026-10-22T15:10:00',
         30000, 350000, 380000,
         N'Chưa thanh toán', N'Chưa xác định', 'NV002');

    INSERT INTO dbo.ChiTietHoaDon (
        MaHoaDon, MaDichVu, TenDichVu, SoLuong, DonGia
    )
    VALUES
        ('HD001', 'DV001', N'Thay dầu máy', 1, 30000),
        ('HD001', 'DV002', N'Thay bugi', 1, 20000),
        ('HD002', 'DV003', N'Thay má phanh trước', 1, 40000),
        ('HD002', 'DV004', N'Thay lọc gió', 1, 20000),
        ('HD003', 'DV005', N'Thay ắc quy', 1, 30000);

    INSERT INTO dbo.PhieuNhap (
        MaPhieuNhap, NgayNhap, MaNhanVienNhap, GhiChu, TongTien
    )
    VALUES
        ('PN001', '2026-10-01T08:00:00', 'NV003',
         N'Nhập dầu nhớt, bugi và má phanh', 3800000),

        ('PN002', '2026-10-10T09:00:00', 'NV003',
         N'Nhập lọc gió, ắc quy và dây curoa', 4440000);

    INSERT INTO dbo.ChiTietPhieuNhap (
        MaPhieuNhap, MaPhuTung, SoLuong, DonGiaNhap
    )
    VALUES
        ('PN001', 'PT001', 30, 80000),
        ('PN001', 'PT002', 20, 25000),
        ('PN001', 'PT003', 15, 60000),
        ('PN002', 'PT004', 20, 35000),
        ('PN002', 'PT005', 8, 280000),
        ('PN002', 'PT006', 10, 150000);

    UPDATE PN
    SET PN.TongTien = ISNULL(Tong.TongTien, 0)
    FROM dbo.PhieuNhap PN
    OUTER APPLY
    (
        SELECT SUM(CT.ThanhTien) AS TongTien
        FROM dbo.ChiTietPhieuNhap CT
        WHERE CT.MaPhieuNhap = PN.MaPhieuNhap
    ) Tong;

    COMMIT TRANSACTION;

    UPDATE PT
    SET PT.SoLuongTon = ISNULL(Tong.SoLuong, 0)
    FROM dbo.PhuTung PT
    OUTER APPLY
    (
        SELECT SUM(CT2.SoLuong) AS SoLuong
        FROM dbo.ChiTietPhieuNhap CT2
        WHERE CT2.MaPhuTung = PT.MaPhuTung
    ) Tong;

    PRINT N'Đã tạo QuanLySuaXe gồm 14 bảng và dữ liệu mẫu.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO


-- =====================================================
-- KIỂM TRA
-- =====================================================

-- Danh sách 14 bảng.
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo'
  AND TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Kiểm tra cột còn cho phép NULL.
-- Kết quả đúng: không có dòng nào.
SELECT
    t.name AS TenBang,
    c.name AS TenCot
FROM sys.tables t
JOIN sys.columns c ON c.object_id = t.object_id
WHERE t.schema_id = SCHEMA_ID(N'dbo')
  AND c.is_nullable = 1;

-- Nhà cung cấp và hạn sử dụng lấy từ bảng PhuTung.
SELECT
    ct.MaPhieuNhap,
    ct.MaPhuTung,
    pt.TenPhuTung,
    pt.MaNCC,
    ncc.TenNCC,
    ncc.SoDienThoai,
    ct.SoLuong,
    ct.DonGiaNhap,
    ct.ThanhTien,
    pt.HanSuDung
FROM dbo.ChiTietPhieuNhap ct
JOIN dbo.PhuTung pt ON pt.MaPhuTung = ct.MaPhuTung
JOIN dbo.NhaCungCap ncc ON ncc.MaNCC = pt.MaNCC
ORDER BY ct.MaPhieuNhap, ct.MaChiTietPhieuNhap;

-- Danh sách tài khoản mẫu.
SELECT
    nv.MaNhanVien,
    nv.TenDangNhap,
    nv.TenNhanVien,
    cv.TenChucVu,
    nv.TrangThai
FROM dbo.NhanVien nv
JOIN dbo.ChucVu cv ON cv.MaChucVu = nv.MaChucVu
ORDER BY nv.MaNhanVien;

-- Tổng tiền hóa đơn.
SELECT
    MaHoaDon,
    MaPhieuTiepNhan,
    TongTienDichVu,
    TongTienPhuTung,
    TongTien,
    TrangThaiThanhToan
FROM dbo.HoaDon
ORDER BY MaHoaDon;
GO


select * from NhanVien where MaNhanVien = 'NV001'
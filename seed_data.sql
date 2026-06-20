-- Dữ liệu mẫu (Seed Data) cho hệ thống VDCD
-- Chạy đoạn script này trong pgAdmin hoặc công cụ quản lý PostgreSQL của bạn để có dữ liệu dùng thử ngay.

-- 1. Thêm dữ liệu Người dùng (Tài khoản mặc định: admin / admin)
INSERT INTO "users" ("userid", "username", "email", "role", "passwordhash", "createddate", "createdby") 
VALUES ('11111111-1111-1111-1111-111111111111', 'admin', 'admin@example.com', 'Admin', 'admin', CURRENT_TIMESTAMP, '11111111-1111-1111-1111-111111111111')
ON CONFLICT ("userid") DO NOTHING;

-- 2. Thêm dữ liệu Loại sản phẩm (Category)
INSERT INTO "category" ("categoryid", "categoryname", "createddate", "createdby") VALUES
('22222222-2222-2222-2222-222222222221', 'Gốm Bát Tràng', CURRENT_TIMESTAMP, '11111111-1111-1111-1111-111111111111'),
('22222222-2222-2222-2222-222222222222', 'Gốm Chu Đậu', CURRENT_TIMESTAMP, '11111111-1111-1111-1111-111111111111'),
('22222222-2222-2222-2222-222222222223', 'Gốm Phù Lãng', CURRENT_TIMESTAMP, '11111111-1111-1111-1111-111111111111')
ON CONFLICT ("categoryid") DO NOTHING;

-- 3. Thêm dữ liệu Đơn vị tính (Unit)
INSERT INTO "unit" ("unitid", "unitname", "createddate", "createdby") VALUES
('33333333-3333-3333-3333-333333333331', 'Cái', CURRENT_TIMESTAMP, '11111111-1111-1111-1111-111111111111'),
('33333333-3333-3333-3333-333333333332', 'Bộ', CURRENT_TIMESTAMP, '11111111-1111-1111-1111-111111111111'),
('33333333-3333-3333-3333-333333333333', 'Chiếc', CURRENT_TIMESTAMP, '11111111-1111-1111-1111-111111111111')
ON CONFLICT ("unitid") DO NOTHING;

-- 4. Thêm dữ liệu Sản phẩm (Product)
INSERT INTO "product" ("productid", "productname", "price", "quantity", "description", "categoryid", "unitid", "createddate", "createdby") VALUES
('44444444-4444-4444-4444-444444444441', 'Lọ hoa Bát Tràng họa tiết hoa sen', 250000, 50, 'Lọ hoa gốm sứ cao cấp, vẽ tay thủ công.', '22222222-2222-2222-2222-222222222221', '33333333-3333-3333-3333-333333333331', CURRENT_TIMESTAMP, '11111111-1111-1111-1111-111111111111'),
('44444444-4444-4444-4444-444444444442', 'Bộ ấm chén Chu Đậu men rạn', 850000, 20, 'Bộ ấm chén uống trà đẳng cấp, men rạn truyền thống.', '22222222-2222-2222-2222-222222222222', '33333333-3333-3333-3333-333333333332', CURRENT_TIMESTAMP, '11111111-1111-1111-1111-111111111111'),
('44444444-4444-4444-4444-444444444443', 'Bình tài lộc Phù Lãng', 1200000, 10, 'Bình gốm mộc, phong thủy.', '22222222-2222-2222-2222-222222222223', '33333333-3333-3333-3333-333333333333', CURRENT_TIMESTAMP, '11111111-1111-1111-1111-111111111111')
ON CONFLICT ("productid") DO NOTHING;

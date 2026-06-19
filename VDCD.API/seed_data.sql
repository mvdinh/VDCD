-- 1. Thêm User (Admin Hệ Thống)
INSERT INTO "users" (userid, username, email, passwordhash, role, createddate)
VALUES 
('11111111-1111-1111-1111-111111111111', 'admin_he_thong', 'admin@vdcd.com', 'hashed_pwd_here', 'Admin', '2026-06-20 00:00:00')
ON CONFLICT (userid) DO NOTHING;

-- 2. Thêm Danh Mục (Category)
INSERT INTO "category" (categoryid, categoryname, createddate, createdby)
VALUES 
('22222222-2222-2222-2222-222222222222', 'Gốm Chu Đậu', '2026-06-20 00:00:00', '11111111-1111-1111-1111-111111111111'),
('33333333-3333-3333-3333-333333333333', 'Gốm Bát Tràng', '2026-06-20 00:00:00', '11111111-1111-1111-1111-111111111111')
ON CONFLICT (categoryid) DO NOTHING;

-- 3. Thêm Đơn Vị Tính (Unit)
INSERT INTO "unit" (unitid, unitname, createddate, createdby)
VALUES 
('44444444-4444-4444-4444-444444444444', 'Chiếc', '2026-06-20 00:00:00', '11111111-1111-1111-1111-111111111111'),
('44444444-4444-4444-4444-444444444445', 'Bộ', '2026-06-20 00:00:00', '11111111-1111-1111-1111-111111111111')
ON CONFLICT (unitid) DO NOTHING;

-- 4. Thêm Sản Phẩm (Product)
INSERT INTO "product" (productid, categoryid, productname, price, quantity, unitid, description, createddate, createdby)
VALUES 
('55555555-5555-5555-5555-555555555555', '22222222-2222-2222-2222-222222222222', 'Bình Gốm Chu Đậu Tứ Quý', 2500000, 100, '44444444-4444-4444-4444-444444444444', 'Bình gốm cao cấp', '2026-06-20 00:00:00', '11111111-1111-1111-1111-111111111111'),
('66666666-6666-6666-6666-666666666666', '33333333-3333-3333-3333-333333333333', 'Bộ Cốc Bát Tràng Men Ngọc', 500000, 200, '44444444-4444-4444-4444-444444444445', 'Bộ 6 chiếc cốc', '2026-06-20 00:00:00', '11111111-1111-1111-1111-111111111111')
ON CONFLICT (productid) DO NOTHING;

-- 5. Thêm Khách Hàng (Customer)
INSERT INTO "customer" (customerid, customername, phonenumber, createddate, createdby)
VALUES 
('77777777-7777-7777-7777-777777777777', 'Nguyễn Văn Khách Hàng', '0987654321', '2026-06-20 00:00:00', '11111111-1111-1111-1111-111111111111')
ON CONFLICT (customerid) DO NOTHING;

-- 6. Thêm Đơn Hàng 1 (Ngày 20/06/2026) - Bán 2 Bình Chu Đậu
INSERT INTO "orders" (orderid, customerid, orderdate, totalamount, discount, paymentmethod, note, createddate, createdby)
VALUES 
('88888888-8888-8888-8888-888888888881', '77777777-7777-7777-7777-777777777777', '2026-06-20 08:30:00', 5000000, 0, 'Tiền mặt', 'Đơn hàng 1', '2026-06-20 08:30:00', '11111111-1111-1111-1111-111111111111')
ON CONFLICT (orderid) DO NOTHING;

INSERT INTO "order_detail" (orderdetailid, orderid, productid, quantity, unitprice, createddate, createdby)
VALUES 
('99999999-9999-9999-9999-999999999991', '88888888-8888-8888-8888-888888888881', '55555555-5555-5555-5555-555555555555', 2, 2500000, '2026-06-20 08:30:00', '11111111-1111-1111-1111-111111111111')
ON CONFLICT (orderdetailid) DO NOTHING;

-- 7. Thêm Đơn Hàng 2 (Ngày 20/06/2026) - Bán 3 Bộ Cốc Bát Tràng
INSERT INTO "orders" (orderid, customerid, orderdate, totalamount, discount, paymentmethod, note, createddate, createdby)
VALUES 
('88888888-8888-8888-8888-888888888882', '77777777-7777-7777-7777-777777777777', '2026-06-20 15:45:00', 1500000, 0, 'Chuyển khoản', 'Đơn hàng 2', '2026-06-20 15:45:00', '11111111-1111-1111-1111-111111111111')
ON CONFLICT (orderid) DO NOTHING;

INSERT INTO "order_detail" (orderdetailid, orderid, productid, quantity, unitprice, createddate, createdby)
VALUES 
('99999999-9999-9999-9999-999999999992', '88888888-8888-8888-8888-888888888882', '66666666-6666-6666-6666-666666666666', 3, 500000, '2026-06-20 15:45:00', '11111111-1111-1111-1111-111111111111')
ON CONFLICT (orderdetailid) DO NOTHING;

-- 8. Thêm Đơn Hàng 3 (Ngày 21/06/2026) - Bán 1 Bình Chu Đậu (Để test lọc theo ngày 20)
INSERT INTO "orders" (orderid, customerid, orderdate, totalamount, discount, paymentmethod, note, createddate, createdby)
VALUES 
('88888888-8888-8888-8888-888888888883', '77777777-7777-7777-7777-777777777777', '2026-06-21 09:00:00', 2500000, 0, 'Tiền mặt', 'Đơn hàng 3 (Khác ngày)', '2026-06-21 09:00:00', '11111111-1111-1111-1111-111111111111')
ON CONFLICT (orderid) DO NOTHING;

INSERT INTO "order_detail" (orderdetailid, orderid, productid, quantity, unitprice, createddate, createdby)
VALUES 
('99999999-9999-9999-9999-999999999993', '88888888-8888-8888-8888-888888888883', '55555555-5555-5555-5555-555555555555', 1, 2500000, '2026-06-21 09:00:00', '11111111-1111-1111-1111-111111111111')
ON CONFLICT (orderdetailid) DO NOTHING;

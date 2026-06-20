import React, { useState, useEffect } from "react";
import { saleApi, productApi } from "../api";

const Sales = ({ user }) => {
  const [orders, setOrders] = useState([]);
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedOrder, setSelectedOrder] = useState(null);

  // Form State
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState({
    customerName: "",
    phoneNumber: "",
    discount: 0,
    paymentMethod: "Tiền mặt",
    note: "",
    orderDetails: [],
  });

  const fetchData = async () => {
    try {
      setLoading(true);
      const [salesRes, prodRes] = await Promise.all([
        saleApi.getAll(),
        productApi.getAll(),
      ]);
      setOrders(salesRes.data);
      setProducts(prodRes.data);
      setError(null);
    } catch (err) {
      console.error(err);
      setError("Failed to fetch data");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleAddNew = () => {
    setFormData({
      customerName: "",
      phoneNumber: "",
      discount: 0,
      paymentMethod: "Tiền mặt",
      note: "",
      orderDetails: [],
    });
    setShowForm(true);
  };

  const handleAddProduct = () => {
    if (products.length === 0) {
      alert("Không có sản phẩm nào trong kho để bán!");
      return;
    }
    const selectedIds = formData.orderDetails.map(d => d.productId);
    const availableProduct = products.find(p => !selectedIds.includes(p.productId));
    if (!availableProduct) {
      alert("Đã thêm tất cả sản phẩm vào hóa đơn!");
      return;
    }
    setFormData((prev) => ({
      ...prev,
      orderDetails: [
        ...prev.orderDetails,
        { productId: availableProduct.productId, quantity: 1 },
      ],
    }));
  };

  const handleRemoveProduct = (index) => {
    const newDetails = [...formData.orderDetails];
    newDetails.splice(index, 1);
    setFormData((prev) => ({ ...prev, orderDetails: newDetails }));
  };

  const handleProductChange = (index, field, value) => {
    const newDetails = [...formData.orderDetails];
    newDetails[index][field] = field === "quantity" ? Number(value) : value;
    setFormData((prev) => ({ ...prev, orderDetails: newDetails }));
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "discount" ? Number(value) : value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (formData.orderDetails.length === 0) {
      alert("Vui lòng thêm ít nhất 1 sản phẩm vào hóa đơn.");
      return;
    }
    try {
      const payload = {
        ...formData,
        createdBy: user?.userId || "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      };
      await saleApi.create(payload);
      setShowForm(false);
      fetchData(); // Refresh list after create
    } catch (err) {
      console.error(err);
      alert(
        "Lỗi khi tạo hóa đơn: " +
          JSON.stringify(err.response?.data || err.message),
      );
    }
  };

  if (loading) return <div>Đang tải dữ liệu hóa đơn...</div>;
  if (error) return <div className="text-danger">{error}</div>;

  return (
    <div className="card" style={{ padding: "20px" }}>
      {showForm ? (
        <div>
          <h2>Tạo Hóa Đơn Mới</h2>
          <form
            onSubmit={handleSubmit}
            style={{
              display: "flex",
              flexDirection: "column",
              gap: "15px",
              maxWidth: "800px",
              marginTop: "20px",
            }}
          >
            <div style={{ display: "flex", gap: "10px" }}>
              <div style={{ flex: 1 }}>
                <label>Tên khách hàng (*):</label>
                <input
                  required
                  name="customerName"
                  value={formData.customerName}
                  onChange={handleChange}
                  className="form-control"
                  style={{ width: "100%", padding: "8px" }}
                />
              </div>
              <div style={{ flex: 1 }}>
                <label>Số điện thoại:</label>
                <input
                  name="phoneNumber"
                  value={formData.phoneNumber}
                  onChange={handleChange}
                  className="form-control"
                  style={{ width: "100%", padding: "8px" }}
                />
              </div>
            </div>

            <div style={{ display: "flex", gap: "10px" }}>
              <div style={{ flex: 1 }}>
                <label>Giảm giá (VNĐ):</label>
                <input
                  type="number"
                  min="0"
                  name="discount"
                  value={formData.discount}
                  onChange={handleChange}
                  className="form-control"
                  style={{ width: "100%", padding: "8px" }}
                />
              </div>
              <div style={{ flex: 1 }}>
                <label>Phương thức thanh toán (*):</label>
                <select
                  required
                  name="paymentMethod"
                  value={formData.paymentMethod}
                  onChange={handleChange}
                  className="form-control"
                  style={{ width: "100%", padding: "8px" }}
                >
                  <option value="Tiền mặt">Tiền mặt</option>
                  <option value="Chuyển khoản">Chuyển khoản</option>
                  <option value="Thẻ tín dụng">Thẻ tín dụng</option>
                </select>
              </div>
            </div>

            <div>
              <label>Ghi chú:</label>
              <textarea
                name="note"
                value={formData.note}
                onChange={handleChange}
                className="form-control"
                style={{ width: "100%", padding: "8px", minHeight: "60px" }}
              />
            </div>

            <hr />
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
              }}
            >
              <h3>Chi tiết sản phẩm</h3>
              <button
                type="button"
                className="btn btn-primary"
                onClick={handleAddProduct}
                style={{ padding: "4px 12px", cursor: "pointer" }}
              >
                + Thêm Sản phẩm
              </button>
            </div>

            <table
              className="table"
              style={{
                width: "100%",
                borderCollapse: "collapse",
                marginTop: "10px",
              }}
            >
              <thead>
                <tr style={{ backgroundColor: "#f8f9fa" }}>
                  <th
                    style={{
                      padding: "10px",
                      borderBottom: "2px solid #dee2e6",
                    }}
                  >
                    Sản phẩm
                  </th>
                  <th
                    style={{
                      padding: "10px",
                      borderBottom: "2px solid #dee2e6",
                      width: "150px",
                    }}
                  >
                    Số lượng
                  </th>
                  <th
                    style={{
                      padding: "10px",
                      borderBottom: "2px solid #dee2e6",
                      width: "100px",
                      textAlign: "center",
                    }}
                  >
                    Xóa
                  </th>
                </tr>
              </thead>
              <tbody>
                {formData.orderDetails.map((detail, index) => (
                  <tr key={index}>
                    <td
                      style={{
                        padding: "10px",
                        borderBottom: "1px solid #dee2e6",
                      }}
                    >
                      <select
                        required
                        value={detail.productId}
                        onChange={(e) =>
                          handleProductChange(
                            index,
                            "productId",
                            e.target.value,
                          )
                        }
                        className="form-control"
                        style={{ width: "100%", padding: "8px" }}
                      >
                        {products.map((p) => {
                          const isSelectedByOther = formData.orderDetails.some(
                            (d, i) => i !== index && d.productId === p.productId
                          );
                          return (
                            <option key={p.productId} value={p.productId} disabled={isSelectedByOther}>
                              {p.productName} ({p.price?.toLocaleString()}đ)
                            </option>
                          );
                        })}
                      </select>
                    </td>
                    <td
                      style={{
                        padding: "10px",
                        borderBottom: "1px solid #dee2e6",
                      }}
                    >
                      <input
                        required
                        type="number"
                        min="1"
                        value={detail.quantity}
                        onChange={(e) =>
                          handleProductChange(index, "quantity", e.target.value)
                        }
                        className="form-control"
                        style={{ width: "100%", padding: "8px" }}
                      />
                    </td>
                    <td
                      style={{
                        padding: "10px",
                        borderBottom: "1px solid #dee2e6",
                        textAlign: "center",
                      }}
                    >
                      <button
                        type="button"
                        className="btn btn-danger"
                        onClick={() => handleRemoveProduct(index)}
                        style={{ padding: "6px 12px", cursor: "pointer" }}
                      >
                        &times;
                      </button>
                    </td>
                  </tr>
                ))}
                {formData.orderDetails.length === 0 && (
                  <tr>
                    <td
                      colSpan="3"
                      style={{
                        textAlign: "center",
                        padding: "20px",
                        color: "gray",
                        borderBottom: "1px solid #dee2e6",
                      }}
                    >
                      Chưa có sản phẩm nào. Vui lòng bấm Thêm Sản phẩm.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>

            <div style={{ display: "flex", gap: "10px", marginTop: "20px" }}>
              <button
                type="submit"
                className="btn btn-primary"
                style={{ padding: "8px 20px", cursor: "pointer" }}
              >
                Lưu Hóa Đơn
              </button>
              <button
                type="button"
                className="btn btn-secondary"
                onClick={() => setShowForm(false)}
                style={{ padding: "8px 20px", cursor: "pointer" }}
              >
                Hủy
              </button>
            </div>
          </form>
        </div>
      ) : (
        <>
          <div
            className="page-header"
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              marginBottom: "20px",
            }}
          >
            <h1 className="page-title">Hóa Đơn Bán Hàng</h1>
            <button
              className="btn btn-primary"
              onClick={handleAddNew}
              style={{ padding: "8px 16px", cursor: "pointer" }}
            >
              + Tạo Hóa Đơn
            </button>
          </div>

          <div className="table-container" style={{ overflowX: "auto" }}>
            <table
              className="table"
              style={{
                width: "100%",
                borderCollapse: "collapse",
                textAlign: "left",
              }}
            >
              <thead>
                <tr style={{ backgroundColor: "#f8f9fa" }}>
                  <th
                    style={{
                      padding: "10px",
                      borderBottom: "2px solid #dee2e6",
                    }}
                  >
                    Mã HĐ
                  </th>
                  <th
                    style={{
                      padding: "10px",
                      borderBottom: "2px solid #dee2e6",
                    }}
                  >
                    Ngày
                  </th>
                  <th
                    style={{
                      padding: "10px",
                      borderBottom: "2px solid #dee2e6",
                    }}
                  >
                    Khách Hàng
                  </th>
                  <th
                    style={{
                      padding: "10px",
                      borderBottom: "2px solid #dee2e6",
                    }}
                  >
                    Tổng Tiền (VNĐ)
                  </th>
                  <th
                    style={{
                      padding: "10px",
                      borderBottom: "2px solid #dee2e6",
                    }}
                  >
                    Hành Động
                  </th>
                </tr>
              </thead>
              <tbody>
                {orders.length === 0 ? (
                  <tr>
                    <td
                      colSpan="5"
                      style={{ textAlign: "center", padding: "20px" }}
                    >
                      Không có hóa đơn nào.
                    </td>
                  </tr>
                ) : (
                  orders.map((order) => (
                    <tr
                      key={order.orderId}
                      style={{ borderBottom: "1px solid #dee2e6" }}
                    >
                      <td style={{ padding: "10px" }}>
                        {order.orderId.substring(0, 8)}
                      </td>
                      <td style={{ padding: "10px" }}>
                        {new Date(order.orderDate).toLocaleString("vi-VN")}
                      </td>
                      <td style={{ padding: "10px" }}>
                        {order.customerName || "N/A"}
                      </td>
                      <td style={{ padding: "10px", fontWeight: "bold" }}>
                        {order.finalAmount?.toLocaleString()} đ
                      </td>
                      <td style={{ padding: "10px" }}>
                        <button
                          className="btn btn-secondary"
                          style={{
                            padding: "4px 8px",
                            fontSize: "0.85rem",
                            cursor: "pointer",
                          }}
                          onClick={() => setSelectedOrder(order)}
                        >
                          Chi tiết
                        </button>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </>
      )}

      {selectedOrder && (
        <div
          style={{
            position: "fixed",
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            backgroundColor: "rgba(0,0,0,0.5)",
            zIndex: 1000,
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
          }}
        >
          <div
            style={{
              backgroundColor: "white",
              padding: "25px",
              borderRadius: "8px",
              minWidth: "600px",
              maxWidth: "90%",
              maxHeight: "90vh",
              overflowY: "auto",
              boxShadow: "0 4px 12px rgba(0,0,0,0.15)",
            }}
          >
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                borderBottom: "1px solid #eee",
                paddingBottom: "15px",
                marginBottom: "20px",
              }}
            >
              <h2 style={{ margin: 0, color: "#333" }}>
                Chi Tiết Hóa Đơn{" "}
                <span style={{ color: "#666", fontSize: "1rem" }}>
                  #{selectedOrder.orderId.substring(0, 8).toUpperCase()}
                </span>
              </h2>
              <button
                onClick={() => setSelectedOrder(null)}
                style={{
                  background: "none",
                  border: "none",
                  fontSize: "1.5rem",
                  cursor: "pointer",
                  color: "#666",
                }}
              >
                &times;
              </button>
            </div>

            <div
              style={{
                marginBottom: "25px",
                display: "flex",
                gap: "40px",
                backgroundColor: "#f8f9fa",
                padding: "15px",
                borderRadius: "6px",
              }}
            >
              <div style={{ flex: 1 }}>
                <p style={{ margin: "5px 0" }}>
                  <strong>Khách hàng:</strong> {selectedOrder.customerName}
                </p>
                <p style={{ margin: "5px 0" }}>
                  <strong>Số điện thoại:</strong>{" "}
                  {selectedOrder.phoneNumber || "N/A"}
                </p>
                <p style={{ margin: "5px 0" }}>
                  <strong>Ngày tạo:</strong>{" "}
                  {new Date(selectedOrder.orderDate).toLocaleString("vi-VN")}
                </p>
                <p style={{ margin: "5px 0" }}>
                  <strong>Người tạo:</strong>{" "}
                  {selectedOrder.createdBy || "N/A"}
                </p>
              </div>
              <div style={{ flex: 1 }}>
                <p style={{ margin: "5px 0" }}>
                  <strong>Thanh toán:</strong> {selectedOrder.paymentMethod}
                </p>
                <p style={{ margin: "5px 0" }}>
                  <strong>Giảm giá:</strong>{" "}
                  {selectedOrder.discount?.toLocaleString()} đ
                </p>
                <p
                  style={{
                    margin: "5px 0",
                    fontSize: "1.1rem",
                    color: "#d9534f",
                  }}
                >
                  <strong>Tổng cộng:</strong>{" "}
                  {selectedOrder.finalAmount?.toLocaleString()} đ
                </p>
              </div>
            </div>

            <table
              className="table"
              style={{
                width: "100%",
                borderCollapse: "collapse",
                textAlign: "left",
              }}
            >
              <thead>
                <tr style={{ backgroundColor: "#e9ecef" }}>
                  <th
                    style={{
                      padding: "12px",
                      borderBottom: "2px solid #dee2e6",
                    }}
                  >
                    Tên Sản Phẩm
                  </th>
                  <th
                    style={{
                      padding: "12px",
                      borderBottom: "2px solid #dee2e6",
                      textAlign: "right",
                    }}
                  >
                    Đơn Giá
                  </th>
                  <th
                    style={{
                      padding: "12px",
                      borderBottom: "2px solid #dee2e6",
                      textAlign: "center",
                    }}
                  >
                    Số Lượng
                  </th>
                  <th
                    style={{
                      padding: "12px",
                      borderBottom: "2px solid #dee2e6",
                      textAlign: "right",
                    }}
                  >
                    Thành Tiền
                  </th>
                </tr>
              </thead>
              <tbody>
                {selectedOrder.orderDetails.map((detail, idx) => (
                  <tr key={idx} style={{ borderBottom: "1px solid #dee2e6" }}>
                    <td style={{ padding: "12px" }}>{detail.productName}</td>
                    <td style={{ padding: "12px", textAlign: "right" }}>
                      {detail.unitPrice?.toLocaleString()} đ
                    </td>
                    <td style={{ padding: "12px", textAlign: "center" }}>
                      {detail.quantity}
                    </td>
                    <td
                      style={{
                        padding: "12px",
                        textAlign: "right",
                        fontWeight: "bold",
                      }}
                    >
                      {detail.subTotal?.toLocaleString()} đ
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>

            <div
              style={{
                marginTop: "25px",
                textAlign: "right",
                borderTop: "1px solid #eee",
                paddingTop: "15px",
              }}
            >
              <button
                className="btn btn-secondary"
                onClick={() => setSelectedOrder(null)}
                style={{ padding: "8px 25px", cursor: "pointer" }}
              >
                Đóng
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Sales;

import React, { useState, useEffect } from 'react';
import { statisticsApi, productApi } from '../api';

const Statistics = () => {
  const [products, setProducts] = useState([]);
  
  // Section 1 State
  const [catFromDate, setCatFromDate] = useState('');
  const [catToDate, setCatToDate] = useState('');
  const [categorySales, setCategorySales] = useState([]);
  const [catLoading, setCatLoading] = useState(false);
  const [catError, setCatError] = useState(null);

  // Section 2 State
  const [selectedProductId, setSelectedProductId] = useState('');
  const [prodFromDate, setProdFromDate] = useState('');
  const [prodToDate, setProdToDate] = useState('');
  const [productRevenue, setProductRevenue] = useState(null);
  const [prodLoading, setProdLoading] = useState(false);
  const [prodError, setProdError] = useState(null);

  useEffect(() => {
    // Load products for dropdown
    productApi.getAll().then(res => setProducts(res.data)).catch(err => console.error(err));
  }, []);

  const handleFetchCategorySales = async () => {
    try {
      setCatLoading(true);
      const params = {};
      if (catFromDate) params.fromDate = new Date(catFromDate).toISOString();
      if (catToDate) params.toDate = new Date(catToDate).toISOString();
      
      const response = await statisticsApi.getCategorySales(params);
      setCategorySales(response.data);
      setCatError(null);
    } catch (err) {
      console.error(err);
      setCatError('Lỗi khi tải thống kê sản lượng.');
    } finally {
      setCatLoading(false);
    }
  };

  const handleFetchProductRevenue = async () => {
    if (!selectedProductId) {
      setProdError("Vui lòng chọn một sản phẩm.");
      return;
    }
    try {
      setProdLoading(true);
      const params = {};
      if (prodFromDate) params.fromDate = new Date(prodFromDate).toISOString();
      if (prodToDate) params.toDate = new Date(prodToDate).toISOString();
      
      const response = await statisticsApi.getProductRevenue(selectedProductId, params);
      setProductRevenue(response.data);
      setProdError(null);
    } catch (err) {
      console.error(err);
      if (err.response && err.response.status === 404) {
        setProductRevenue({ notFound: true });
        setProdError(null);
      } else {
        setProdError('Lỗi khi tải doanh thu sản phẩm.');
      }
    } finally {
      setProdLoading(false);
    }
  };

  return (
    <div className="card" style={{ padding: '20px' }}>
      <div className="page-header" style={{ marginBottom: '30px' }}>
        <h1 className="page-title">Báo Cáo Thống Kê</h1>
      </div>

      <div style={{ display: 'flex', gap: '30px', flexDirection: 'column' }}>
        
        {/* Section 1: Thống kê sản lượng */}
        <div className="card" style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.05)', border: '1px solid #e9ecef', padding: '20px' }}>
          <h2 style={{ marginBottom: '20px', color: '#333' }}>1. Thống kê sản lượng theo danh mục</h2>
          
          <div style={{ display: 'flex', gap: '15px', alignItems: 'flex-end', marginBottom: '20px' }}>
            <div style={{ flex: 1 }}>
              <label>Từ ngày:</label>
              <input type="datetime-local" className="form-control" style={{width: '100%', padding: '8px'}} value={catFromDate} onChange={(e) => setCatFromDate(e.target.value)} />
            </div>
            <div style={{ flex: 1 }}>
              <label>Đến ngày:</label>
              <input type="datetime-local" className="form-control" style={{width: '100%', padding: '8px'}} value={catToDate} onChange={(e) => setCatToDate(e.target.value)} />
            </div>
            <div>
              <button className="btn btn-primary" onClick={handleFetchCategorySales} style={{ padding: '8px 20px', cursor: 'pointer' }}>Tra cứu</button>
            </div>
          </div>

          {catError && <p className="text-danger">{catError}</p>}
          
          <div className="table-container">
            <table className="table" style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left' }}>
              <thead>
                <tr style={{ backgroundColor: '#f8f9fa' }}>
                  <th style={{ padding: '12px', borderBottom: '2px solid #dee2e6' }}>Tên Danh Mục</th>
                  <th style={{ padding: '12px', borderBottom: '2px solid #dee2e6', textAlign: 'center' }}>Số Lượng Đã Bán</th>
                </tr>
              </thead>
              <tbody>
                {catLoading ? (
                  <tr><td colSpan="2" style={{textAlign: 'center', padding: '20px'}}>Đang tải...</td></tr>
                ) : categorySales.length === 0 ? (
                  <tr><td colSpan="2" style={{textAlign: 'center', padding: '20px', color: 'gray'}}>Chưa có dữ liệu thống kê. Hãy chọn thời gian và ấn Tra cứu.</td></tr>
                ) : (
                  categorySales.map((stat, idx) => (
                    <tr key={idx} style={{ borderBottom: '1px solid #dee2e6' }}>
                      <td style={{ padding: '12px' }}>{stat.categoryName || stat.categoryId}</td>
                      <td style={{ padding: '12px', textAlign: 'center', fontWeight: 'bold' }}>{stat.totalQuantitySold}</td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>

        {/* Section 2: Tra cứu doanh số theo sản phẩm */}
        <div className="card" style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.05)', border: '1px solid #e9ecef', padding: '20px' }}>
          <h2 style={{ marginBottom: '20px', color: '#333' }}>2. Tra cứu doanh số theo sản phẩm</h2>
          
          <div style={{ display: 'flex', gap: '15px', alignItems: 'flex-end', marginBottom: '20px' }}>
            <div style={{ flex: 2 }}>
              <label>Chọn sản phẩm (*):</label>
              <select className="form-control" style={{width: '100%', padding: '8px'}} value={selectedProductId} onChange={(e) => setSelectedProductId(e.target.value)}>
                <option value="">-- Tìm kiếm / Chọn sản phẩm --</option>
                {products.map(p => (
                  <option key={p.productId} value={p.productId}>{p.productName}</option>
                ))}
              </select>
            </div>
            <div style={{ flex: 1 }}>
              <label>Từ ngày:</label>
              <input type="datetime-local" className="form-control" style={{width: '100%', padding: '8px'}} value={prodFromDate} onChange={(e) => setProdFromDate(e.target.value)} />
            </div>
            <div style={{ flex: 1 }}>
              <label>Đến ngày:</label>
              <input type="datetime-local" className="form-control" style={{width: '100%', padding: '8px'}} value={prodToDate} onChange={(e) => setProdToDate(e.target.value)} />
            </div>
            <div>
              <button className="btn btn-primary" onClick={handleFetchProductRevenue} style={{ padding: '8px 20px', cursor: 'pointer' }}>Tra cứu</button>
            </div>
          </div>

          {prodError && <p className="text-danger">{prodError}</p>}

          {prodLoading ? (
            <p>Đang tải...</p>
          ) : productRevenue ? (
            productRevenue.notFound || productRevenue.totalQuantitySold === 0 ? (
               <div style={{ padding: '20px', backgroundColor: '#fff3cd', border: '1px solid #ffeeba', borderRadius: '4px', color: '#856404' }}>
                 Sản phẩm này chưa bán được cái nào trong khoảng thời gian đã chọn!
               </div>
            ) : (
              <div style={{ display: 'flex', gap: '20px', backgroundColor: '#e8f4fd', padding: '20px', borderRadius: '8px', border: '1px solid #b8daff' }}>
                <div style={{ flex: 1 }}>
                  <h3 style={{ margin: '0 0 10px 0', color: '#0056b3' }}>{productRevenue.productName}</h3>
                  <p style={{ margin: '5px 0' }}>Từ: {new Date(productRevenue.fromDate).toLocaleString('vi-VN')}</p>
                  <p style={{ margin: '5px 0' }}>Đến: {new Date(productRevenue.toDate).toLocaleString('vi-VN')}</p>
                </div>
                <div style={{ flex: 1, textAlign: 'right', display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
                  <p style={{ margin: '0 0 5px 0', fontSize: '1.2rem' }}>Số lượng đã bán: <strong>{productRevenue.totalQuantitySold}</strong></p>
                  <p style={{ margin: 0, fontSize: '1.5rem', color: '#d9534f', fontWeight: 'bold' }}>Doanh thu: {productRevenue.totalRevenue?.toLocaleString()} đ</p>
                </div>
              </div>
            )
          ) : (
            <p style={{ color: 'gray', padding: '20px', textAlign: 'center', backgroundColor: '#f8f9fa', borderRadius: '4px' }}>
              Hãy chọn một sản phẩm và bấm "Tra cứu" để xem doanh thu.
            </p>
          )}

        </div>

      </div>
    </div>
  );
};

export default Statistics;

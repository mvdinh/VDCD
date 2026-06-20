import React, { useState, useEffect } from 'react';
import { productApi, categoryApi, unitApi } from '../api';

const Products = ({ user }) => {
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [units, setUnits] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Form State
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState({
    productId: null,
    productName: '',
    categoryId: '',
    price: 0,
    quantity: 0,
    unitId: '',
    description: '',
    createdBy: user?.userId || '',
    modifiedBy: user?.userId || ''
  });

  const fetchProducts = async () => {
    try {
      setLoading(true);
      const response = await productApi.getAll();
      setProducts(response.data);
      setError(null);
    } catch (err) {
      console.error(err);
      setError('Failed to fetch products');
    } finally {
      setLoading(false);
    }
  };

  const fetchInitialData = async () => {
    try {
      setLoading(true);
      const [prodRes, catRes, unitRes] = await Promise.all([
        productApi.getAll(),
        categoryApi.getAll(),
        unitApi.getAll()
      ]);
      setProducts(prodRes.data);
      setCategories(catRes.data);
      setUnits(unitRes.data);
      setError(null);
    } catch (err) {
      console.error(err);
      setError('Failed to fetch initial data');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchInitialData();
  }, []);

  const handleDelete = async (id) => {
    if (window.confirm('Bạn có chắc muốn xóa sản phẩm này?')) {
      try {
        await productApi.delete(id);
        fetchProducts(); // Refresh list
      } catch (err) {
        console.error(err);
        alert('Lỗi khi xóa: ' + (err.response?.data?.message || err.response?.data?.devMsg || err.message));
      }
    }
  };

  const handleEdit = (product) => {
    const category = categories.find(c => c.categoryName === product.categoryName);
    const unit = units.find(u => u.unitName === product.unitName);

    setFormData({
      productId: product.productId,
      productName: product.productName,
      categoryId: category ? category.categoryId : '',
      price: product.price,
      quantity: product.quantity,
      unitId: unit ? unit.unitId : '',
      description: product.description || '',
      createdBy: user.userId,
      modifiedBy: user.userId
    });
    setShowForm(true);
  };

  const handleAddNew = () => {
    setFormData({
      productId: null,
      productName: '',
      categoryId: categories.length > 0 ? categories[0].categoryId : '',
      price: 0,
      quantity: 0,
      unitId: units.length > 0 ? units[0].unitId : '',
      description: '',
      createdBy: user.userId,
      modifiedBy: user.userId
    });
    setShowForm(true);
  };

  const handleSave = async (e) => {
    e.preventDefault();
    try {
      const payload = { ...formData };
      
      if (formData.productId) {
        await productApi.update(formData.productId, payload);
      } else {
        await productApi.create(payload);
      }
      setShowForm(false);
      fetchProducts();
    } catch (err) {
      console.error(err);
      alert('Lỗi khi lưu sản phẩm: ' + JSON.stringify(err.response?.data || err.message));
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'quantity' ? Number(value) : value
    }));
  };

  const handlePriceChange = (e) => {
    const rawValue = e.target.value.replace(/\D/g, '');
    const numberValue = rawValue ? parseInt(rawValue, 10) : 0;
    setFormData(prev => ({
      ...prev,
      price: numberValue
    }));
  };

  const tdStyle = {
    padding: '12px',
    whiteSpace: 'nowrap',
    overflow: 'hidden',
    textOverflow: 'ellipsis'
  };

  if (loading) return <div>Đang tải sản phẩm...</div>;
  if (error) return <div className="text-danger">{error}</div>;

  return (
    <div className="card" style={{ padding: '20px' }}>
      {showForm ? (
        <div>
          <h2>{formData.productId ? 'Sửa Sản Phẩm' : 'Thêm Sản Phẩm Mới'}</h2>
          <form onSubmit={handleSave} style={{ display: 'flex', flexDirection: 'column', gap: '20px', maxWidth: '700px', marginTop: '20px', backgroundColor: '#f8f9fa', padding: '30px', borderRadius: '8px', border: '1px solid #dee2e6', boxShadow: '0 4px 6px rgba(0,0,0,0.05)' }}>
            
            <div>
              <label style={{ fontWeight: '500', marginBottom: '8px', display: 'block' }}>Tên sản phẩm (*):</label>
              <input required name="productName" value={formData.productName} onChange={handleChange} className="form-control" style={{width: '100%', padding: '10px 12px'}} />
            </div>
            
            <div style={{ display: 'flex', gap: '20px' }}>
              <div style={{ flex: 1 }}>
                <label style={{ fontWeight: '500', marginBottom: '8px', display: 'block' }}>Loại sản phẩm (*):</label>
                <select required name="categoryId" value={formData.categoryId} onChange={handleChange} className="form-control" style={{width: '100%', padding: '10px 12px'}}>
                  <option value="">-- Chọn loại sản phẩm --</option>
                  {categories.map(c => (
                    <option key={c.categoryId} value={c.categoryId}>{c.categoryName}</option>
                  ))}
                </select>
              </div>
              <div style={{ flex: 1 }}>
                <label style={{ fontWeight: '500', marginBottom: '8px', display: 'block' }}>Đơn vị tính (*):</label>
                <select required name="unitId" value={formData.unitId} onChange={handleChange} className="form-control" style={{width: '100%', padding: '10px 12px'}}>
                  <option value="">-- Chọn đơn vị --</option>
                  {units.map(u => (
                    <option key={u.unitId} value={u.unitId}>{u.unitName}</option>
                  ))}
                </select>
              </div>
            </div>

            <div style={{ display: 'flex', gap: '20px' }}>
              <div style={{ flex: 1 }}>
                <label style={{ fontWeight: '500', marginBottom: '8px', display: 'block' }}>Giá (VNĐ):</label>
                <input type="text" required name="price" value={formData.price === 0 ? '' : formData.price.toLocaleString('vi-VN')} onChange={handlePriceChange} className="form-control" placeholder="0" style={{width: '100%', padding: '10px 12px', textAlign: 'right'}} />
              </div>
              <div style={{ flex: 1 }}>
                <label style={{ fontWeight: '500', marginBottom: '8px', display: 'block' }}>Số lượng:</label>
                <input type="number" required min="0" name="quantity" value={formData.quantity} onChange={handleChange} className="form-control" style={{width: '100%', padding: '10px 12px', textAlign: 'right'}} />
              </div>
            </div>

            <div>
              <label style={{ fontWeight: '500', marginBottom: '8px', display: 'block' }}>Mô tả:</label>
              <textarea name="description" value={formData.description} onChange={handleChange} className="form-control" style={{width: '100%', padding: '10px 12px', minHeight: '100px'}} />
            </div>

            <div style={{ display: 'flex', gap: '15px', marginTop: '10px', justifyContent: 'flex-end' }}>
              <button type="button" className="btn btn-secondary" onClick={() => setShowForm(false)} style={{ padding: '10px 24px', cursor: 'pointer', borderRadius: '4px', fontWeight: '500' }}>Hủy</button>
              <button type="submit" className="btn btn-primary" style={{ padding: '10px 24px', cursor: 'pointer', borderRadius: '4px', fontWeight: '500' }}>Lưu Sản Phẩm</button>
            </div>
          </form>
        </div>
      ) : (
        <>
          <div className="page-header" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
            <h1 className="page-title">Quản lý Sản Phẩm</h1>
            <button className="btn btn-primary" onClick={handleAddNew} style={{ padding: '8px 16px', cursor: 'pointer' }}>+ Thêm Sản Phẩm</button>
          </div>

          <div className="table-container" style={{ overflowX: 'auto', borderRadius: '8px', boxShadow: '0 4px 12px rgba(0,0,0,0.05)', border: '1px solid #e9ecef', backgroundColor: '#fff' }}>
            <table className="table" style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left', tableLayout: 'fixed' }}>
              <thead>
                <tr style={{ backgroundColor: '#f8f9fa', borderBottom: '2px solid #dee2e6', color: '#495057' }}>
                  <th style={{ padding: '15px 12px', width: '20%' }}>Tên SP</th>
                  <th style={{ padding: '15px 12px', width: '15%' }}>Loại Sản Phẩm</th>
                  <th style={{ padding: '15px 12px', width: '10%' }}>Đơn Vị</th>
                  <th style={{ padding: '15px 12px', width: '12%' }}>Giá</th>
                  <th style={{ padding: '15px 12px', width: '8%' }}>SL</th>
                  <th style={{ padding: '15px 12px', width: '20%' }}>Mô Tả</th>
                  <th style={{ padding: '15px 12px', width: '12%' }}>Ngày Tạo</th>
                  <th style={{ padding: '15px 12px', width: '15%' }}>Hành Động</th>
                </tr>
              </thead>
              <tbody>
                {products.length === 0 ? (
                  <tr><td colSpan="8" style={{ textAlign: 'center', padding: '30px', color: '#6c757d' }}>Không có sản phẩm nào.</td></tr>
                ) : (
                  products.map((product) => (
                    <tr 
                      key={product.productId} 
                      style={{ borderBottom: '1px solid #eee', transition: 'background-color 0.2s ease' }} 
                      onMouseEnter={(e) => e.currentTarget.style.backgroundColor = '#f8f9fa'} 
                      onMouseLeave={(e) => e.currentTarget.style.backgroundColor = 'transparent'}
                    >
                      <td style={tdStyle} title={product.productName}>{product.productName}</td>
                      <td style={tdStyle} title={product.categoryName}>{product.categoryName}</td>
                      <td style={tdStyle} title={product.unitName}>{product.unitName}</td>
                      <td style={tdStyle} title={product.price?.toLocaleString() + ' đ'}>{product.price?.toLocaleString()} đ</td>
                      <td style={tdStyle} title={product.quantity}>{product.quantity}</td>
                      <td style={tdStyle} title={product.description}>{product.description}</td>
                      <td style={tdStyle} title={product.createdDate ? new Date(product.createdDate).toLocaleDateString('vi-VN') : ''}>{product.createdDate ? new Date(product.createdDate).toLocaleDateString('vi-VN') : ''}</td>
                      <td style={{ padding: '10px 12px', whiteSpace: 'nowrap' }}>
                        <div style={{ display: 'flex', gap: '0.5rem' }}>
                          <button className="btn btn-secondary" style={{ padding: '6px 12px', fontSize: '0.85rem', cursor: 'pointer', borderRadius: '4px', border: '1px solid #ced4da', backgroundColor: '#fff', color: '#495057' }} onClick={() => handleEdit(product)}>Sửa</button>
                          <button 
                            className="btn btn-danger" 
                            style={{ padding: '6px 12px', fontSize: '0.85rem', cursor: 'pointer', borderRadius: '4px', border: 'none', backgroundColor: '#dc3545', color: '#fff' }}
                            onClick={() => handleDelete(product.productId)}
                          >
                            Xóa
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </>
      )}
    </div>
  );
};

export default Products;

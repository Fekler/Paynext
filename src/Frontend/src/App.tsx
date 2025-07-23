import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './pages/login';
import Navbar from './Navbar';
import UsuariosList from './pages/UsuariosList';
import { useSelector } from 'react-redux';
import type { RootState } from './store';

const ProtectedRoute: React.FC<{ children: React.ReactNode, allowedRoles: string[] }> = ({ children, allowedRoles }) => {
  const user = useSelector((state: RootState) => state.auth.user);
  if (!user) return <Navigate to="/login" replace />;
  if (!allowedRoles.includes(user.role)) return <Navigate to="/login" replace />;
  return <>{children}</>;
};

function App() {
  const user = useSelector((state: RootState) => state.auth.user);
  return (
    <Router>
      {user && <Navbar />}
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/usuarios" element={
          <ProtectedRoute allowedRoles={["Admin"]}>
            <UsuariosList />
          </ProtectedRoute>
        } />
        <Route path="*" element={<Navigate to={user ? (user.role === 'Admin' ? '/usuarios' : '/meu-contrato') : '/login'} replace />} />
      </Routes>
    </Router>
  );
}

export default App;

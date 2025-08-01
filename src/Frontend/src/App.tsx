import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './pages/login';
import Navbar from './Navbar';
import UsuariosList from './pages/UsuariosList';
import ContractsList from './pages/ContractsList';
import ContractDetail from './pages/ContractDetail';
import MeusContratos from './pages/MeusContratos';
import ClientRequests from './pages/MinhasSolicitacoes';
import Requests from './pages/Requests';
import { useSelector } from 'react-redux';
import type { RootState } from './store';
import Box from '@mui/material/Box';

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
      <Box sx={{ pt: { xs: '56px', sm: '64px' } }}>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/usuarios" element={
            <ProtectedRoute allowedRoles={["Admin"]}>
              <UsuariosList />
            </ProtectedRoute>
          } />
          <Route path="/contratos" element={
            <ProtectedRoute allowedRoles={["Admin"]}>
              <ContractsList />
            </ProtectedRoute>
          } />
                <Route path="/Requests" element={
          <ProtectedRoute allowedRoles={["Admin"]}>
            <Requests />
          </ProtectedRoute>
        } />
        <Route path="/contratos/:uuid" element={
          <ProtectedRoute allowedRoles={["Admin", "Client"]}>
            <ContractDetail />
          </ProtectedRoute>
        } />
        <Route path="/meus-contratos" element={
          <ProtectedRoute allowedRoles={["Client"]}>
            <MeusContratos />
          </ProtectedRoute>
        } />
        <Route path="/meus-parcelamentos" element={
          <ProtectedRoute allowedRoles={["Client"]}>
            <ClientRequests />
          </ProtectedRoute>
        } />
        <Route path="*" element={<Navigate to={user ? (user.role === 'Admin' ? '/usuarios' : '/meus-contratos') : '/login'} replace />} />
      </Routes>
    </Box>
    </Router>
  );
}

export default App;

import React from 'react';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import { useSelector, useDispatch } from 'react-redux';
import type { RootState } from './store';
import { useLocation, useNavigate } from 'react-router-dom';
import { logout } from './authSlice';

const adminTabs = [
  { label: 'Overview', path: '/overview' },
  { label: 'Contratos', path: '/contratos' },
  { label: 'Usuários', path: '/usuarios' },
  { label: 'Parcelamentos', path: '/parcelamentos' },
];

const clientTabs = [
  { label: 'Meus Contratos', path: '/meus-contratos' },
  { label: 'Minhas Solicitações', path: '/meus-parcelamentos' },
];

const Navbar: React.FC = () => {
  const user = useSelector((state: RootState) => state.auth.user);
  const location = useLocation();
  const navigate = useNavigate();
  const dispatch = useDispatch();

  if (!user) return null;

  const tabs = user.role === 'Admin' ? adminTabs : clientTabs;
  const currentTab = tabs.findIndex(tab => location.pathname.startsWith(tab.path));

  return (
    <AppBar position="static" color="primary">
      <Toolbar className="flex flex-col md:flex-row md:justify-between md:items-center">
        <Typography variant="h6" className="mb-2 md:mb-0">Paynext</Typography>
        <Tabs
          value={currentTab === -1 ? 0 : currentTab}
          onChange={(_, idx) => navigate(tabs[idx].path)}
          textColor="inherit"
          indicatorColor="secondary"
        >
          {tabs.map(tab => (
            <Tab key={tab.path} label={tab.label} />
          ))}
        </Tabs>
        <button
          onClick={() => {
            dispatch(logout());
            navigate('/login');
          }}
          style={{ marginLeft: 16, background: 'white', color: '#1976d2', border: 'none', borderRadius: 4, padding: '6px 16px', fontWeight: 600, cursor: 'pointer' }}
        >
          Logout
        </button>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar; 
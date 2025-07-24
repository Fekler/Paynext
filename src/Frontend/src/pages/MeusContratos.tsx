import React, { useEffect, useState } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography, CircularProgress, IconButton, Tooltip, Snackbar, Alert as MuiAlert, TextField, Button } from '@mui/material';
import { Visibility } from '@mui/icons-material';
import api from '../api';
import { useNavigate } from 'react-router-dom';
import SearchIcon from '@mui/icons-material/Search';

interface Contract {
  uuid: string;
  contractNumber: string;
  userUuid: string;
  initialAmount: number;
  remainingValue: number;
  installmentsCount: number;
  startDate: string;
  endDate: string;
  isFinished: boolean;
  isActive: boolean;
  installments: any[];
}

const MeusContratos: React.FC = () => {
  const [contracts, setContracts] = useState<Contract[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [snackbar, setSnackbar] = useState<{open: boolean, message: string, severity: 'success'|'error'}>({open: false, message: '', severity: 'success'});
  const [search, setSearch] = useState('');
  const [searching, setSearching] = useState(false);
  const navigate = useNavigate();

  const fetchContracts = async () => {
    setLoading(true);
    try {
      const response = await api.get('/api/v1/Contracts');
      setContracts(response.data.data || response.data);
    } catch (err: any) {
      setError('Erro ao buscar contratos');
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = async () => {
    if (!search.trim()) {
      fetchContracts();
      return;
    }
    setSearching(true);
    try {
      const result = await api.get(`/api/v1/Contracts/contractNumber/${search}`);
      setContracts(result.data.data ? [result.data.data] : result.data ? [result.data] : []);
    } catch {
      setContracts([]);
    } finally {
      setSearching(false);
    }
  };

  useEffect(() => {
    fetchContracts();
  }, []);

  const filteredContracts = contracts;

  if (loading) return <div className="flex justify-center mt-10"><CircularProgress /></div>;
  if (error) return <Typography color="error">{error}</Typography>;
  if (!contracts || contracts.length === 0) {
    return (
      <div className="min-h-screen w-screen flex flex-col items-center justify-center bg-gray-100">
        <div className="w-full max-w-6xl flex flex-col md:flex-row gap-4 mb-4 px-2 md:px-8">
          <TextField
            label="Buscar por Número do Contrato"
            variant="outlined"
            value={search}
            onChange={e => setSearch(e.target.value)}
            onKeyDown={e => { if (e.key === 'Enter') handleSearch(); }}
            fullWidth
          />
          <Button variant="contained" color="primary" startIcon={<SearchIcon />} onClick={handleSearch} disabled={searching}>
            Buscar
          </Button>
          <Button variant="outlined" color="secondary" onClick={() => { setSearch(''); fetchContracts(); }}>
            Voltar
          </Button>
        </div>
        <Typography variant="h6" color='black' className="mt-20 text-center">Nenhum contrato encontrado.</Typography>
      </div>
    );
  }

  return (
    <div className="min-h-screen w-screen flex flex-col items-center bg-gray-100">
      <div className="flex flex-col md:flex-row justify-between items-center w-full max-w-6xl mt-8 mb-4 gap-4 px-2 md:px-8">
        <Typography variant="h6" color='black'>Meus Contratos</Typography>
      </div>
      <div className="w-full max-w-6xl flex flex-col md:flex-row gap-4 mb-4 px-2 md:px-8">
        <TextField
          label="Buscar por Número do Contrato"
          variant="outlined"
          value={search}
          onChange={e => setSearch(e.target.value)}
          onKeyDown={e => { if (e.key === 'Enter') handleSearch(); }}
          fullWidth
        />
        <Button variant="contained" color="primary" startIcon={<SearchIcon />} onClick={handleSearch} disabled={searching}>
          Buscar
        </Button>
      </div>
      <TableContainer
        component={Paper}
        className="w-full max-w-6xl overflow-x-auto"
        style={{ marginBottom: 32 }}
      >
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Número</TableCell>
              <TableCell>Valor Inicial</TableCell>
              <TableCell>Parcelas</TableCell>
              <TableCell>Início</TableCell>
              <TableCell>Fim</TableCell>
              <TableCell>Ativo</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {filteredContracts.map((contract) => (
              <TableRow key={contract.uuid}>
                <TableCell>{contract.contractNumber}</TableCell>
                <TableCell>{contract.initialAmount.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</TableCell>
                <TableCell>{contract.installmentsCount ?? 0}</TableCell>
                <TableCell>{contract.startDate && new Date(contract.startDate).toLocaleDateString()}</TableCell>
                <TableCell>{contract.endDate && new Date(contract.endDate).toLocaleDateString()}</TableCell>
                <TableCell>{contract.isActive ? 'Sim' : 'Não'}</TableCell>
                <TableCell>
                  <Tooltip title="Ver Detalhes"><IconButton onClick={() => navigate(`/contratos/${contract.uuid}`)}><Visibility /></IconButton></Tooltip>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <Snackbar open={snackbar.open} autoHideDuration={3000} onClose={() => setSnackbar({...snackbar, open: false})}>
        <MuiAlert onClose={() => setSnackbar({...snackbar, open: false})} severity={snackbar.severity} sx={{ width: '100%' }}>
          {snackbar.message}
        </MuiAlert>
      </Snackbar>
    </div>
  );
};

export default MeusContratos; 
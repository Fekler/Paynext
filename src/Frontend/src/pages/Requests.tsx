import React, { useEffect, useState } from 'react';
import { Typography, Paper, List, ListItem, ListItemText, CircularProgress, Box, Pagination, Button, Snackbar, IconButton } from '@mui/material';
import MuiAlert from '@mui/material/Alert';
import { ArrowBack, Close, RadioButtonChecked } from '@mui/icons-material';
import Tooltip from '@mui/material/Tooltip';
import { contractsService } from '../services/contractsService';

const Requests: React.FC = () => {
  const [solicitacoes, setSolicitacoes] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [total, setTotal] = useState(0);
  const [approved, setApproved] = useState<Set<string>>(new Set());
  const [rejected, setRejected] = useState<Set<string>>(new Set());
  const [snackbar, setSnackbar] = useState<{open: boolean, message: string, severity: 'success'|'error'}>({open: false, message: '', severity: 'success'});

  const fetchSolicitacoes = async (pagerNumber = 1) => {
    setLoading(true);
    try {
      const response = await contractsService.getAdvancedRequests(pagerNumber, pageSize);
      setSolicitacoes(response.data || []);
      setTotal(response.totalCount || response.data?.length || 0);
    } catch (err: any) {
      setError('Erro ao buscar solicitações');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSolicitacoes(page);
    setApproved(new Set());
    setRejected(new Set());
    // eslint-disable-next-line
  }, [page]);

  // Lista de todos os pendentes
  const pendentes: any[] = [];
  solicitacoes.forEach(sol => {
    sol.installments.forEach((inst: any) => {
      if (inst.antecipationStatus === 1 && !approved.has(inst.uuid) && !rejected.has(inst.uuid)) {
        pendentes.push({ ...inst, contractNumber: sol.contractNumber, clientName: sol.clientName });
      }
    });
  });

  // Lista de aprovados
  const aprovados: any[] = [];
  solicitacoes.forEach(sol => {
    sol.installments.forEach((inst: any) => {
      if (approved.has(inst.uuid)) {
        aprovados.push({ ...inst, contractNumber: sol.contractNumber, clientName: sol.clientName });
      }
    });
  });

  // Lista de rejeitados
  const rejeitados: any[] = [];
  solicitacoes.forEach(sol => {
    sol.installments.forEach((inst: any) => {
      if (rejected.has(inst.uuid)) {
        rejeitados.push({ ...inst, contractNumber: sol.contractNumber, clientName: sol.clientName });
      }
    });
  });

  const moveToApproved = (uuid: string) => {
    setApproved(prev => new Set(prev).add(uuid));
    setRejected(prev => { const s = new Set(prev); s.delete(uuid); return s; });
  };
  const moveToRejected = (uuid: string) => {
    setRejected(prev => new Set(prev).add(uuid));
    setApproved(prev => { const s = new Set(prev); s.delete(uuid); return s; });
  };
  const moveToPending = (uuid: string) => {
    setApproved(prev => { const s = new Set(prev); s.delete(uuid); return s; });
    setRejected(prev => { const s = new Set(prev); s.delete(uuid); return s; });
  };

  const handleSend = async () => {
    const actions = [
      ...Array.from(approved).map(uuid => ({ InstallmentUuid: uuid, IsAccepted: true })),
      ...Array.from(rejected).map(uuid => ({ InstallmentUuid: uuid, IsAccepted: false })),
    ];
    if (actions.length === 0) return;
    try {
      const result = await contractsService.approveAdvancedRequests(actions);
      setSnackbar({open: true, message: result?.message || 'Ação realizada com sucesso!', severity: result?.ok === false ? 'error' : 'success'});
      setApproved(new Set());
      setRejected(new Set());
      fetchSolicitacoes(page);
    } catch (err: any) {
      let msg = 'Erro ao processar solicitações.';
      if (err?.response?.data?.message) msg = err.response.data.message;
      setSnackbar({open: true, message: msg, severity: 'error'});
    }
  };

  if (loading) return <div className="flex justify-center mt-10"><CircularProgress /></div>;
  if (error) return <Typography color="error">{error}</Typography>;

  return (
    <Box className="min-h-screen w-screen flex flex-col items-center bg-gray-100 py-8" style={{ color: '#111' }}>
      <Paper className="w-full max-w-6xl p-8 mb-8">
        <Typography variant="h5" gutterBottom>Solicitações de Antecipação</Typography>
        <Box
          sx={{
            display: 'flex',
            flexDirection: { xs: 'column', md: 'row' },
            gap: 4,
            width: '100%',
          }}
        >
          <Box flex={1}>
            <Typography variant="subtitle1" fontWeight={600} mb={2}>Pendentes</Typography>
            <List>
              {pendentes.length === 0 && <Typography color="textSecondary">Nenhuma pendente</Typography>}
              {pendentes.map((inst, i) => (
                <ListItem key={inst.uuid} divider className={i % 2 === 0 ? 'bg-gray-50' : 'bg-white'}>
                  <ListItemText
                    primary={<>
                      <span className="font-semibold">Contrato: {inst.contractNumber}</span><br/>
                      <span>Cliente: {inst.clientName}</span><br/>
                      <span>Vencimento: {new Date(inst.dueDate).toLocaleDateString()}</span><br/>
                      <span>Valor: {Number(inst.value).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</span>
                    </>}
                  />
                  <Box>
                    <Tooltip title="Aprovar solicitação">
                      <IconButton color="success" onClick={() => moveToApproved(inst.uuid)}>
                        <RadioButtonChecked />
                      </IconButton>
                    </Tooltip>
                    <Tooltip title="Rejeitar solicitação">
                      <IconButton color="error" onClick={() => moveToRejected(inst.uuid)}>
                        <Close />
                      </IconButton>
                    </Tooltip>
                  </Box>
                </ListItem>
              ))}
            </List>
          </Box>
          <Box flex={1}>
            <Typography variant="subtitle1" fontWeight={600} mb={2}>Aprovados</Typography>
            <List>
              {aprovados.length === 0 && <Typography color="textSecondary">Nenhum aprovado</Typography>}
              {aprovados.map((inst, i) => (
                <ListItem key={inst.uuid} divider className={i % 2 === 0 ? 'bg-gray-50' : 'bg-white'}>
                  <ListItemText
                    primary={<>
                      <span className="font-semibold">Contrato: {inst.contractNumber}</span><br/>
                      <span>Cliente: {inst.clientName}</span><br/>
                      <span>Vencimento: {new Date(inst.dueDate).toLocaleDateString()}</span><br/>
                      <span>Valor: {Number(inst.value).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</span>
                    </>}
                  />
                  <Tooltip title="Mover para pendente">
                    <IconButton color="warning" onClick={() => moveToPending(inst.uuid)}>
                      <ArrowBack />
                    </IconButton>
                  </Tooltip>
                </ListItem>
              ))}
            </List>
          </Box>
          <Box flex={1}>
            <Typography variant="subtitle1" fontWeight={600} mb={2}>Rejeitados</Typography>
            <List>
              {rejeitados.length === 0 && <Typography color="textSecondary">Nenhum rejeitado</Typography>}
              {rejeitados.map((inst, i) => (
                <ListItem key={inst.uuid} divider className={i % 2 === 0 ? 'bg-gray-50' : 'bg-white'}>
                  <ListItemText
                    primary={<>
                      <span className="font-semibold">Contrato: {inst.contractNumber}</span><br/>
                      <span>Cliente: {inst.clientName}</span><br/>
                      <span>Vencimento: {new Date(inst.dueDate).toLocaleDateString()}</span><br/>
                      <span>Valor: {Number(inst.value).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}</span>
                    </>}
                  />
                  <Tooltip title="Mover para pendente">
                    <IconButton color="warning" onClick={() => moveToPending(inst.uuid)}>
                      <ArrowBack />
                    </IconButton>
                  </Tooltip>
                </ListItem>
              ))}
            </List>
          </Box>
        </Box>
        <Box className="flex gap-4 mt-8 justify-center">
          <Button
            variant="contained"
            color="primary"
            disabled={approved.size === 0 && rejected.size === 0}
            onClick={handleSend}
            size="large"
          >
            Enviar Decisões
          </Button>
        </Box>
        {total > pageSize && (
          <Box className="flex justify-center mt-4">
            <Pagination count={Math.ceil(total / pageSize)} page={page} onChange={(_, value) => setPage(value)} color="primary" />
          </Box>
        )}
      </Paper>
      <Snackbar open={snackbar.open} autoHideDuration={3000} onClose={() => setSnackbar({...snackbar, open: false})}>
        <MuiAlert onClose={() => setSnackbar({...snackbar, open: false})} severity={snackbar.severity} sx={{ width: '100%' }}>
          {snackbar.message}
        </MuiAlert>
      </Snackbar>
    </Box>
  );
};

export default Requests; 
import { useState, useEffect } from 'react'
import {
  Box,
  AppBar,
  Toolbar,
  Typography,
  Button,
  Container,
  Grid,
  Card,
  CardContent,
  TextField,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  Drawer,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  CssBaseline,
  ThemeProvider,
  createTheme,
  Alert,
  Snackbar,
  CircularProgress,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Chip,
  Fab,
  Tooltip
} from '@mui/material'
import {
  Dashboard as DashboardIcon,
  People as PeopleIcon,
  Description as PolicyIcon,
  Warning as ClaimsIcon,
  Payment as PaymentIcon,
  Folder as DocumentsIcon,
  Login as LoginIcon,
  Logout as LogoutIcon,
  Security as SecurityIcon,
  TrendingUp as TrendingUpIcon,
  AccountBalance as AccountBalanceIcon,
  Notifications as NotificationsIcon,
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Visibility as ViewIcon,
  AttachMoney as MoneyIcon,
  Description as DocumentIcon
} from '@mui/icons-material'

import { apiService, User, Customer, Policy, Claim } from './services/api'

// Add Payment and Document interfaces
interface Payment {
  id: number
  paymentNumber: string
  policyId: number
  customerId: number
  amount: number
  paymentDate: string
  paymentMethod: string
  status: string
  description?: string
  createdAt: string
}

interface Document {
  id: number
  documentNumber: string
  customerId: number
  policyId?: number
  documentType: string
  fileName: string
  fileSize: number
  uploadDate: string
  status: string
  description?: string
  createdAt: string
}

const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
  },
})

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const [currentUser, setCurrentUser] = useState<User | null>(null)
  const [activeSection, setActiveSection] = useState('dashboard')
  const [customers, setCustomers] = useState<Customer[]>([])
  const [policies, setPolicies] = useState<Policy[]>([])
  const [claims, setClaims] = useState<Claim[]>([])
  const [payments, setPayments] = useState<Payment[]>([])
  const [documents, setDocuments] = useState<Document[]>([])
  const [openSnackbar, setOpenSnackbar] = useState(false)
  const [snackbarMessage, setSnackbarMessage] = useState('')
  const [loading, setLoading] = useState(false)
  const [loginForm, setLoginForm] = useState({ username: '', password: '' })
  
  // Modal states
  const [openModal, setOpenModal] = useState(false)
  const [modalType, setModalType] = useState('')
  const [selectedItem, setSelectedItem] = useState<any>(null)
  const [formData, setFormData] = useState<any>({})

  // Check for existing token on app start
  useEffect(() => {
    const token = apiService.getToken()
    if (token) {
      checkAuthStatus()
    }
  }, [])

  // Load data when authenticated
  useEffect(() => {
    if (isAuthenticated) {
      loadData()
    }
  }, [isAuthenticated])

  const checkAuthStatus = async () => {
    try {
      const user = await apiService.getCurrentUser()
      setCurrentUser(user)
      setIsAuthenticated(true)
    } catch (error) {
      apiService.clearToken()
      setIsAuthenticated(false)
    }
  }

  const loadData = async () => {
    setLoading(true)
    try {
      const [customersData, policiesData, claimsData] = await Promise.all([
        apiService.getCustomers(),
        apiService.getPolicies(),
        apiService.getClaims()
      ])
      setCustomers(customersData)
      setPolicies(policiesData)
      setClaims(claimsData)
      
      // Mock data for payments and documents
      setPayments([
        { id: 1, paymentNumber: 'PAY-001', policyId: 1, customerId: 1, amount: 2500, paymentDate: '2024-01-15', paymentMethod: 'Credit Card', status: 'Completed', createdAt: '2024-01-15' },
        { id: 2, paymentNumber: 'PAY-002', policyId: 2, customerId: 2, amount: 1800, paymentDate: '2024-02-01', paymentMethod: 'Bank Transfer', status: 'Completed', createdAt: '2024-02-01' },
        { id: 3, paymentNumber: 'PAY-003', policyId: 3, customerId: 3, amount: 5000, paymentDate: '2024-03-01', paymentMethod: 'Cash', status: 'Pending', createdAt: '2024-03-01' }
      ])
      
      setDocuments([
        { id: 1, documentNumber: 'DOC-001', customerId: 1, policyId: 1, documentType: 'Policy Document', fileName: 'policy_001.pdf', fileSize: 1024, uploadDate: '2024-01-01', status: 'Active', createdAt: '2024-01-01' },
        { id: 2, documentNumber: 'DOC-002', customerId: 2, policyId: 2, documentType: 'Claim Form', fileName: 'claim_002.pdf', fileSize: 2048, uploadDate: '2024-07-20', status: 'Active', createdAt: '2024-07-20' },
        { id: 3, documentNumber: 'DOC-003', customerId: 3, policyId: 3, documentType: 'Invoice', fileName: 'invoice_003.pdf', fileSize: 512, uploadDate: '2024-03-01', status: 'Active', createdAt: '2024-03-01' }
      ])
      
      setSnackbarMessage('Demo modunda çalışıyor - Backend bağlantısı yok')
      setOpenSnackbar(true)
    } catch (error) {
      console.error('Failed to load data:', error)
      // Use mock data if API is not available
      setCustomers([
        { id: 1, firstName: 'Ahmet', lastName: 'Yılmaz', email: 'ahmet@example.com', phoneNumber: '0532 123 4567', address: 'İstanbul, Türkiye', dateOfBirth: '1985-05-15', nationalId: '12345678901', createdAt: '2024-01-01' },
        { id: 2, firstName: 'Ayşe', lastName: 'Demir', email: 'ayse@example.com', phoneNumber: '0533 987 6543', address: 'Ankara, Türkiye', dateOfBirth: '1990-08-22', nationalId: '12345678902', createdAt: '2024-01-01' },
        { id: 3, firstName: 'ABC', lastName: 'Şirketi', email: 'info@abc.com', phoneNumber: '0212 555 1234', address: 'İstanbul, Türkiye', dateOfBirth: '2000-01-01', nationalId: '12345678903', createdAt: '2024-01-01' }
      ])
      setPolicies([
        { id: 1, policyNumber: 'POL-001', customerId: 1, insuranceType: 'Auto', productName: 'Kasko Sigortası', premiumAmount: 2500, coverageAmount: 100000, startDate: '2024-01-01', endDate: '2025-01-01', status: 'Active', createdAt: '2024-01-01' },
        { id: 2, policyNumber: 'POL-002', customerId: 2, insuranceType: 'Health', productName: 'Sağlık Sigortası', premiumAmount: 1800, coverageAmount: 50000, startDate: '2024-02-01', endDate: '2025-02-01', status: 'Active', createdAt: '2024-02-01' },
        { id: 3, policyNumber: 'POL-003', customerId: 3, insuranceType: 'Business', productName: 'İşyeri Sigortası', premiumAmount: 5000, coverageAmount: 200000, startDate: '2024-03-01', endDate: '2025-03-01', status: 'Active', createdAt: '2024-03-01' }
      ])
      setClaims([
        { id: 1, claimNumber: 'HAS-001', policyId: 1, customerId: 1, claimType: 'AutoAccident', description: 'Trafik kazası', incidentDate: '2024-06-15', reportedDate: '2024-06-15', claimAmount: 15000, status: 'UnderReview', createdAt: '2024-06-15' },
        { id: 2, claimNumber: 'HAS-002', policyId: 2, customerId: 2, claimType: 'Medical', description: 'Sağlık gideri', incidentDate: '2024-07-20', reportedDate: '2024-07-20', claimAmount: 3000, status: 'Paid', createdAt: '2024-07-20' }
      ])
      setPayments([
        { id: 1, paymentNumber: 'PAY-001', policyId: 1, customerId: 1, amount: 2500, paymentDate: '2024-01-15', paymentMethod: 'Credit Card', status: 'Completed', createdAt: '2024-01-15' },
        { id: 2, paymentNumber: 'PAY-002', policyId: 2, customerId: 2, amount: 1800, paymentDate: '2024-02-01', paymentMethod: 'Bank Transfer', status: 'Completed', createdAt: '2024-02-01' },
        { id: 3, paymentNumber: 'PAY-003', policyId: 3, customerId: 3, amount: 5000, paymentDate: '2024-03-01', paymentMethod: 'Cash', status: 'Pending', createdAt: '2024-03-01' }
      ])
      setDocuments([
        { id: 1, documentNumber: 'DOC-001', customerId: 1, policyId: 1, documentType: 'Policy Document', fileName: 'policy_001.pdf', fileSize: 1024, uploadDate: '2024-01-01', status: 'Active', createdAt: '2024-01-01' },
        { id: 2, documentNumber: 'DOC-002', customerId: 2, policyId: 2, documentType: 'Claim Form', fileName: 'claim_002.pdf', fileSize: 2048, uploadDate: '2024-07-20', status: 'Active', createdAt: '2024-07-20' },
        { id: 3, documentNumber: 'DOC-003', customerId: 3, policyId: 3, documentType: 'Invoice', fileName: 'invoice_003.pdf', fileSize: 512, uploadDate: '2024-03-01', status: 'Active', createdAt: '2024-03-01' }
      ])
      setSnackbarMessage('Demo modunda çalışıyor - Backend bağlantısı yok')
      setOpenSnackbar(true)
    } finally {
      setLoading(false)
    }
  }

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    
    try {
      // Try real login first
      const response = await apiService.login(loginForm)
      apiService.setToken(response.token)
      setCurrentUser({ 
        id: '1', 
        userName: loginForm.username, 
        email: 'admin@sigorta.com',
        roles: response.roles 
      })
      setIsAuthenticated(true)
      setSnackbarMessage('Başarıyla giriş yapıldı!')
      setOpenSnackbar(true)
    } catch (error) {
      console.error('Login failed:', error)
      // Demo login if backend is not available
      if (loginForm.username === 'admin' && loginForm.password === 'Admin123!') {
        setCurrentUser({ 
          id: '1', 
          userName: 'admin', 
          email: 'admin@sigorta.com',
          roles: ['Admin'] 
        })
        setIsAuthenticated(true)
        setSnackbarMessage('Demo modunda giriş yapıldı!')
        setOpenSnackbar(true)
      } else {
        setSnackbarMessage('Giriş başarısız! Kullanıcı adı: admin, Şifre: Admin123!')
        setOpenSnackbar(true)
      }
    } finally {
      setLoading(false)
    }
  }

  const handleLogout = () => {
    apiService.clearToken()
    setIsAuthenticated(false)
    setCurrentUser(null)
    setActiveSection('dashboard')
    setSnackbarMessage('Çıkış yapıldı')
    setOpenSnackbar(true)
  }

  const menuItems = [
    { text: 'Dashboard', icon: <DashboardIcon />, section: 'dashboard' },
    { text: 'Müşteriler', icon: <PeopleIcon />, section: 'customers' },
    { text: 'Poliçeler', icon: <PolicyIcon />, section: 'policies' },
    { text: 'Hasarlar', icon: <ClaimsIcon />, section: 'claims' },
    { text: 'Ödemeler', icon: <PaymentIcon />, section: 'payments' },
    { text: 'Dökümanlar', icon: <DocumentsIcon />, section: 'documents' },
  ]

  const handleOpenModal = (type: string, item?: any) => {
    setModalType(type)
    setSelectedItem(item)
    setFormData(item || {})
    setOpenModal(true)
  }

  const handleCloseModal = () => {
    setOpenModal(false)
    setModalType('')
    setSelectedItem(null)
    setFormData({})
  }

  const handleSave = () => {
    // Mock save operation
    setSnackbarMessage('İşlem başarıyla kaydedildi!')
    setOpenSnackbar(true)
    handleCloseModal()
  }

  const handleDelete = (type: string, id: number) => {
    // Mock delete operation
    setSnackbarMessage('Kayıt başarıyla silindi!')
    setOpenSnackbar(true)
  }

  const renderLoginForm = () => (
    <Box
      sx={{
        minHeight: '100vh',
        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        p: 2
      }}
    >
      <Paper
        elevation={8}
        sx={{
          p: 4,
          maxWidth: 400,
          width: '100%',
          textAlign: 'center'
        }}
      >
        <SecurityIcon sx={{ fontSize: 60, color: 'primary.main', mb: 2 }} />
        <Typography variant="h4" component="h1" gutterBottom>
          Sigorta Platformu
        </Typography>
        <Typography variant="body1" color="text.secondary" sx={{ mb: 3 }}>
          Giriş yapın
        </Typography>
        
        <Box component="form" onSubmit={handleLogin} sx={{ mt: 1 }}>
          <TextField
            margin="normal"
            required
            fullWidth
            id="username"
            label="Kullanıcı Adı"
            name="username"
            autoComplete="username"
            autoFocus
            value={loginForm.username}
            onChange={(e) => setLoginForm({ ...loginForm, username: e.target.value })}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            name="password"
            label="Şifre"
            type="password"
            id="password"
            autoComplete="current-password"
            value={loginForm.password}
            onChange={(e) => setLoginForm({ ...loginForm, password: e.target.value })}
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
            startIcon={loading ? <CircularProgress size={20} /> : <LoginIcon />}
            disabled={loading}
          >
            {loading ? 'Giriş Yapılıyor...' : 'Giriş Yap'}
          </Button>
        </Box>
      </Paper>
    </Box>
  )

  const renderNavigation = () => (
    <AppBar position="static">
      <Toolbar>
        <SecurityIcon sx={{ mr: 2 }} />
        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
          Sigorta Platformu
        </Typography>
        <Button color="inherit" onClick={handleLogout} startIcon={<LogoutIcon />}>
          Çıkış
        </Button>
      </Toolbar>
    </AppBar>
  )

  const renderDashboard = () => (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h4" component="h1" gutterBottom>
        Dashboard
      </Typography>
      <Typography variant="body1" color="text.secondary" sx={{ mb: 4 }}>
        Hoş geldiniz! Sigorta yönetim platformunuzun genel durumu
      </Typography>
      
      <Grid container spacing={3} sx={{ mb: 4 }}>
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box sx={{ display: 'flex', alignItems: 'center' }}>
                <PeopleIcon sx={{ fontSize: 40, color: 'primary.main', mr: 2 }} />
                <Box>
                  <Typography variant="h4" component="div">
                    {customers.length}
                  </Typography>
                  <Typography color="text.secondary">
                    Toplam Müşteri
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box sx={{ display: 'flex', alignItems: 'center' }}>
                <PolicyIcon sx={{ fontSize: 40, color: 'success.main', mr: 2 }} />
                <Box>
                  <Typography variant="h4" component="div">
                    {policies.length}
                  </Typography>
                  <Typography color="text.secondary">
                    Aktif Poliçe
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box sx={{ display: 'flex', alignItems: 'center' }}>
                <ClaimsIcon sx={{ fontSize: 40, color: 'warning.main', mr: 2 }} />
                <Box>
                  <Typography variant="h4" component="div">
                    {claims.filter(c => c.status === 'UnderReview').length}
                  </Typography>
                  <Typography color="text.secondary">
                    Açık Hasar
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
        
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Box sx={{ display: 'flex', alignItems: 'center' }}>
                <AccountBalanceIcon sx={{ fontSize: 40, color: 'info.main', mr: 2 }} />
                <Box>
                  <Typography variant="h4" component="div">
                    ₺{policies.reduce((sum, policy) => sum + policy.premiumAmount, 0).toLocaleString()}
                  </Typography>
                  <Typography color="text.secondary">
                    Toplam Prim
                  </Typography>
                </Box>
              </Box>
            </CardContent>
          </Card>
        </Grid>
      </Grid>

      <Card>
        <CardContent>
          <Typography variant="h6" gutterBottom>
            Hızlı İşlemler
          </Typography>
          <Grid container spacing={2}>
            {menuItems.slice(1, 4).map((item) => (
              <Grid item xs={12} sm={4} key={item.section}>
                <Button
                  fullWidth
                  variant="outlined"
                  startIcon={item.icon}
                  onClick={() => setActiveSection(item.section)}
                  sx={{ py: 2 }}
                >
                  {item.text}
                </Button>
              </Grid>
            ))}
          </Grid>
        </CardContent>
      </Card>
    </Container>
  )

  const renderCustomers = () => (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" component="h1">
          Müşteri Yönetimi
        </Typography>
        <Button variant="contained" startIcon={<PeopleIcon />} onClick={() => handleOpenModal('customer')}>
          Yeni Müşteri
        </Button>
      </Box>
      
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Ad Soyad</TableCell>
              <TableCell>E-posta</TableCell>
              <TableCell>Telefon</TableCell>
              <TableCell>Durum</TableCell>
              <TableCell>İşlemler</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {customers.map((customer) => (
              <TableRow key={customer.id}>
                <TableCell>{customer.id}</TableCell>
                <TableCell>{`${customer.firstName} ${customer.lastName}`}</TableCell>
                <TableCell>{customer.email}</TableCell>
                <TableCell>{customer.phoneNumber}</TableCell>
                <TableCell>
                  <Chip
                    label="Müşteri"
                    color="success"
                    size="small"
                  />
                </TableCell>
                <TableCell>
                  <IconButton size="small" color="primary" onClick={() => handleOpenModal('customer', customer)}>
                    <EditIcon />
                  </IconButton>
                  <IconButton size="small" color="error" onClick={() => handleDelete('customer', customer.id)}>
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  )

  const renderPolicies = () => (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" component="h1">
          Poliçe Yönetimi
        </Typography>
        <Button variant="contained" startIcon={<PolicyIcon />} onClick={() => handleOpenModal('policy')}>
          Yeni Poliçe
        </Button>
      </Box>
      
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Poliçe No</TableCell>
              <TableCell>Ürün Adı</TableCell>
              <TableCell>Sigorta Türü</TableCell>
              <TableCell>Başlangıç</TableCell>
              <TableCell>Bitiş</TableCell>
              <TableCell>Prim</TableCell>
              <TableCell>Durum</TableCell>
              <TableCell>İşlemler</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {policies.map((policy) => (
              <TableRow key={policy.id}>
                <TableCell>{policy.policyNumber}</TableCell>
                <TableCell>{policy.productName}</TableCell>
                <TableCell>{policy.insuranceType}</TableCell>
                <TableCell>{new Date(policy.startDate).toLocaleDateString('tr-TR')}</TableCell>
                <TableCell>{new Date(policy.endDate).toLocaleDateString('tr-TR')}</TableCell>
                <TableCell>₺{policy.premiumAmount.toLocaleString()}</TableCell>
                <TableCell>
                  <Chip
                    label={policy.status}
                    color="success"
                    size="small"
                  />
                </TableCell>
                <TableCell>
                  <IconButton size="small" color="primary" onClick={() => handleOpenModal('policy', policy)}>
                    <EditIcon />
                  </IconButton>
                  <IconButton size="small" color="error" onClick={() => handleDelete('policy', policy.id)}>
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  )

  const renderClaims = () => (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" component="h1">
          Hasar Yönetimi
        </Typography>
        <Button variant="contained" startIcon={<ClaimsIcon />} onClick={() => handleOpenModal('claim')}>
          Yeni Hasar
        </Button>
      </Box>
      
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Hasar No</TableCell>
              <TableCell>Hasar Türü</TableCell>
              <TableCell>Açıklama</TableCell>
              <TableCell>Tarih</TableCell>
              <TableCell>Tutar</TableCell>
              <TableCell>Durum</TableCell>
              <TableCell>İşlemler</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {claims.map((claim) => (
              <TableRow key={claim.id}>
                <TableCell>{claim.claimNumber}</TableCell>
                <TableCell>{claim.claimType}</TableCell>
                <TableCell>{claim.description}</TableCell>
                <TableCell>{new Date(claim.incidentDate).toLocaleDateString('tr-TR')}</TableCell>
                <TableCell>₺{claim.claimAmount.toLocaleString()}</TableCell>
                <TableCell>
                  <Chip
                    label={claim.status}
                    color={claim.status === 'UnderReview' ? 'warning' : 'success'}
                    size="small"
                  />
                </TableCell>
                <TableCell>
                  <IconButton size="small" color="primary" onClick={() => handleOpenModal('claim', claim)}>
                    <EditIcon />
                  </IconButton>
                  <IconButton size="small" color="error" onClick={() => handleDelete('claim', claim.id)}>
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  )

  const renderPayments = () => (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" component="h1">
          Ödeme Yönetimi
        </Typography>
        <Button variant="contained" startIcon={<MoneyIcon />} onClick={() => handleOpenModal('payment')}>
          Yeni Ödeme
        </Button>
      </Box>
      
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Ödeme No</TableCell>
              <TableCell>Poliçe No</TableCell>
              <TableCell>Müşteri</TableCell>
              <TableCell>Tutar</TableCell>
              <TableCell>Ödeme Tarihi</TableCell>
              <TableCell>Yöntem</TableCell>
              <TableCell>Durum</TableCell>
              <TableCell>İşlemler</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {payments.map((payment) => {
              const policy = policies.find(p => p.id === payment.policyId)
              const customer = customers.find(c => c.id === payment.customerId)
              return (
                <TableRow key={payment.id}>
                  <TableCell>{payment.paymentNumber}</TableCell>
                  <TableCell>{policy?.policyNumber}</TableCell>
                  <TableCell>{customer ? `${customer.firstName} ${customer.lastName}` : 'N/A'}</TableCell>
                  <TableCell>₺{payment.amount.toLocaleString()}</TableCell>
                  <TableCell>{new Date(payment.paymentDate).toLocaleDateString('tr-TR')}</TableCell>
                  <TableCell>{payment.paymentMethod}</TableCell>
                  <TableCell>
                    <Chip
                      label={payment.status}
                      color={payment.status === 'Completed' ? 'success' : 'warning'}
                      size="small"
                    />
                  </TableCell>
                  <TableCell>
                    <IconButton size="small" color="primary" onClick={() => handleOpenModal('payment', payment)}>
                      <EditIcon />
                    </IconButton>
                    <IconButton size="small" color="error" onClick={() => handleDelete('payment', payment.id)}>
                      <DeleteIcon />
                    </IconButton>
                  </TableCell>
                </TableRow>
              )
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  )

  const renderDocuments = () => (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" component="h1">
          Döküman Yönetimi
        </Typography>
        <Button variant="contained" startIcon={<DocumentIcon />} onClick={() => handleOpenModal('document')}>
          Yeni Döküman
        </Button>
      </Box>
      
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Döküman No</TableCell>
              <TableCell>Dosya Adı</TableCell>
              <TableCell>Döküman Türü</TableCell>
              <TableCell>Müşteri</TableCell>
              <TableCell>Boyut</TableCell>
              <TableCell>Yükleme Tarihi</TableCell>
              <TableCell>Durum</TableCell>
              <TableCell>İşlemler</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {documents.map((document) => {
              const customer = customers.find(c => c.id === document.customerId)
              return (
                <TableRow key={document.id}>
                  <TableCell>{document.documentNumber}</TableCell>
                  <TableCell>{document.fileName}</TableCell>
                  <TableCell>{document.documentType}</TableCell>
                  <TableCell>{customer ? `${customer.firstName} ${customer.lastName}` : 'N/A'}</TableCell>
                  <TableCell>{(document.fileSize / 1024).toFixed(1)} KB</TableCell>
                  <TableCell>{new Date(document.uploadDate).toLocaleDateString('tr-TR')}</TableCell>
                  <TableCell>
                    <Chip
                      label={document.status}
                      color="success"
                      size="small"
                    />
                  </TableCell>
                  <TableCell>
                    <IconButton size="small" color="primary">
                      <ViewIcon />
                    </IconButton>
                    <IconButton size="small" color="error" onClick={() => handleDelete('document', document.id)}>
                      <DeleteIcon />
                    </IconButton>
                  </TableCell>
                </TableRow>
              )
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  )

  const renderModal = () => {
    const getModalTitle = () => {
      switch (modalType) {
        case 'customer': return selectedItem ? 'Müşteri Düzenle' : 'Yeni Müşteri'
        case 'policy': return selectedItem ? 'Poliçe Düzenle' : 'Yeni Poliçe'
        case 'claim': return selectedItem ? 'Hasar Düzenle' : 'Yeni Hasar'
        case 'payment': return selectedItem ? 'Ödeme Düzenle' : 'Yeni Ödeme'
        case 'document': return selectedItem ? 'Döküman Düzenle' : 'Yeni Döküman'
        default: return 'Form'
      }
    }

    const renderForm = () => {
      switch (modalType) {
        case 'customer':
          return (
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField fullWidth label="Ad" value={formData.firstName || ''} onChange={(e) => setFormData({...formData, firstName: e.target.value})} />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField fullWidth label="Soyad" value={formData.lastName || ''} onChange={(e) => setFormData({...formData, lastName: e.target.value})} />
              </Grid>
              <Grid item xs={12}>
                <TextField fullWidth label="E-posta" value={formData.email || ''} onChange={(e) => setFormData({...formData, email: e.target.value})} />
              </Grid>
              <Grid item xs={12}>
                <TextField fullWidth label="Telefon" value={formData.phoneNumber || ''} onChange={(e) => setFormData({...formData, phoneNumber: e.target.value})} />
              </Grid>
              <Grid item xs={12}>
                <TextField fullWidth label="Adres" value={formData.address || ''} onChange={(e) => setFormData({...formData, address: e.target.value})} />
              </Grid>
            </Grid>
          )
        case 'policy':
          return (
            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField fullWidth label="Poliçe No" value={formData.policyNumber || ''} onChange={(e) => setFormData({...formData, policyNumber: e.target.value})} />
              </Grid>
              <Grid item xs={12} sm={6}>
                <FormControl fullWidth>
                  <InputLabel>Sigorta Türü</InputLabel>
                  <Select value={formData.insuranceType || ''} onChange={(e) => setFormData({...formData, insuranceType: e.target.value})}>
                    <MenuItem value="Auto">Araç</MenuItem>
                    <MenuItem value="Health">Sağlık</MenuItem>
                    <MenuItem value="Business">İşyeri</MenuItem>
                  </Select>
                </FormControl>
              </Grid>
              <Grid item xs={12}>
                <TextField fullWidth label="Ürün Adı" value={formData.productName || ''} onChange={(e) => setFormData({...formData, productName: e.target.value})} />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField fullWidth label="Prim Tutarı" type="number" value={formData.premiumAmount || ''} onChange={(e) => setFormData({...formData, premiumAmount: e.target.value})} />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField fullWidth label="Teminat Tutarı" type="number" value={formData.coverageAmount || ''} onChange={(e) => setFormData({...formData, coverageAmount: e.target.value})} />
              </Grid>
            </Grid>
          )
        default:
          return <Typography>Form içeriği burada görünecek</Typography>
      }
    }

    return (
      <Dialog open={openModal} onClose={handleCloseModal} maxWidth="md" fullWidth>
        <DialogTitle>{getModalTitle()}</DialogTitle>
        <DialogContent>
          <Box sx={{ pt: 2 }}>
            {renderForm()}
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseModal}>İptal</Button>
          <Button onClick={handleSave} variant="contained">Kaydet</Button>
        </DialogActions>
      </Dialog>
    )
  }

  if (!isAuthenticated) {
    return (
      <ThemeProvider theme={theme}>
        <CssBaseline />
        {renderLoginForm()}
      </ThemeProvider>
    )
  }

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Box sx={{ display: 'flex' }}>
        <Drawer
          variant="permanent"
          sx={{
            width: 240,
            flexShrink: 0,
            '& .MuiDrawer-paper': {
              width: 240,
              boxSizing: 'border-box',
            },
          }}
        >
          <Toolbar />
          <Box sx={{ overflow: 'auto' }}>
            <List>
              {menuItems.map((item) => (
                <ListItem
                  button
                  key={item.text}
                  selected={activeSection === item.section}
                  onClick={() => setActiveSection(item.section)}
                >
                  <ListItemIcon>{item.icon}</ListItemIcon>
                  <ListItemText primary={item.text} />
                </ListItem>
              ))}
            </List>
          </Box>
        </Drawer>
        <Box component="main" sx={{ flexGrow: 1 }}>
          {renderNavigation()}
          <Toolbar />
          {loading ? (
            <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '50vh' }}>
              <CircularProgress />
            </Box>
          ) : (
            <>
              {activeSection === 'dashboard' && renderDashboard()}
              {activeSection === 'customers' && renderCustomers()}
              {activeSection === 'policies' && renderPolicies()}
              {activeSection === 'claims' && renderClaims()}
              {activeSection === 'payments' && renderPayments()}
              {activeSection === 'documents' && renderDocuments()}
            </>
          )}
        </Box>
      </Box>
      
      {renderModal()}
      
      <Snackbar
        open={openSnackbar}
        autoHideDuration={3000}
        onClose={() => setOpenSnackbar(false)}
      >
        <Alert onClose={() => setOpenSnackbar(false)} severity="success">
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </ThemeProvider>
  )
}

export default App

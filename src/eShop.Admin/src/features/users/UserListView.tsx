import {
    Box,
    Button,
    Card,
    CardContent,
    Chip,
    CircularProgress,
    Container,
    IconButton,
    InputAdornment,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TextField,
    Toolbar,
    Tooltip,
    Typography
} from "@mui/material";
import { useFetchUsersQuery } from "./usersApi";
import { Link } from "react-router-dom";
import { Edit, Refresh, Search, Visibility } from "@mui/icons-material";

export default function UserListView() {
    const { data: users, error, isLoading, refetch } = useFetchUsersQuery();

    const getRoleColor = (role: string) => {
        switch (role.toLowerCase()) {
            case 'admin':
                return 'success';
            default:
                return 'primary';
        }
    };

    // console.log(users);

    return (
        <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
            <Paper sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
                <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <Typography variant="h5" component="h1" gutterBottom>
                        Users Management
                    </Typography>
                    <Button
                        variant="contained"
                        color="primary"
                        component={Link}
                        to="#"
                        sx={{ height: 40 }}
                    >
                        Add New User
                    </Button>
                </Box>

                <Card>
                    <CardContent>
                        <Toolbar sx={{
                            pl: { sm: 2 },
                            pr: { xs: 1, sm: 1 },
                            justifyContent: 'space-between'
                        }}>
                            <TextField
                                variant="outlined"
                                placeholder="Search users..."
                                sx={{ width: '100%', maxWidth: 500 }}
                                slotProps={{
                                    input: {
                                        startAdornment: (
                                            <InputAdornment position="start">
                                                <Search />
                                            </InputAdornment>
                                        )
                                    }
                                }}
                                size="small"
                            />
                            <Tooltip title="Refresh list">
                                <IconButton onClick={() => refetch()}>
                                    <Refresh />
                                </IconButton>
                            </Tooltip>
                        </Toolbar>

                        {isLoading ? (
                            <Box sx={{ display: 'flex', justifyContent: 'center', p: 3 }}>
                                <CircularProgress />
                            </Box>
                        ) : error ? (
                            <Box sx={{ p: 3, textAlign: 'center' }}>
                                <Typography color="error">Error loading users data. Please try again.</Typography>
                                <Button
                                    variant="outlined"
                                    color="primary"
                                    onClick={() => refetch()}
                                    sx={{ mt: 2 }}
                                >
                                    Retry
                                </Button>
                            </Box>
                        ) : (
                            <>
                                <TableContainer>
                                    <Table>
                                        <TableHead>
                                            <TableRow>
                                                <TableCell>Full name</TableCell>
                                                <TableCell>Email</TableCell>
                                                <TableCell>Roles</TableCell>
                                                <TableCell>Joined</TableCell>
                                                <TableCell align="right">Actions</TableCell>
                                            </TableRow>
                                        </TableHead>
                                        <TableBody>
                                            {users && users.length > 0 ? (
                                                users.map((user) => (
                                                    <TableRow key={user.id} hover>
                                                        <TableCell>{`${user.firstName} ${user.lastName}`}</TableCell>
                                                        <TableCell>{user.email}</TableCell>
                                                        <TableCell>
                                                            {user.roles?.map((role) => (
                                                                <Chip
                                                                    key={role}
                                                                    label={role}
                                                                    size="small"
                                                                    color={getRoleColor(role) as any}
                                                                    sx={{ mr: 0.5, mb: 0.5 }}
                                                                />
                                                            ))}
                                                        </TableCell>
                                                        <TableCell>{user.joinedDate ?? "NaN"}</TableCell>
                                                        <TableCell align="right">
                                                            <Tooltip title="View Details">
                                                                <IconButton>
                                                                    <Visibility fontSize="small" />
                                                                </IconButton>
                                                            </Tooltip>
                                                            <Tooltip title="Edit User">
                                                                <IconButton>
                                                                    <Edit fontSize="small" />
                                                                </IconButton>
                                                            </Tooltip>
                                                        </TableCell>
                                                    </TableRow>
                                                ))
                                            ) : (
                                                <TableRow>
                                                    <TableCell colSpan={4} align="center">
                                                        <Typography variant="body2" color="textSecondary" sx={{ py: 2 }}>
                                                            {users?.length === 0 ? 'No users found' : 'No matching users found'}
                                                        </Typography>
                                                    </TableCell>
                                                </TableRow>
                                            )}
                                        </TableBody>
                                    </Table>
                                </TableContainer>
                            </>
                        )}
                    </CardContent>
                </Card>
            </Paper>
        </Container>
    )
}
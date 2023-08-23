import styles from './index.css'

import {BrowserRouter} from "react-router-dom";
import AppRoutes from "./AppRoutes";
import { QueryClient, QueryClientProvider } from 'react-query'

import NavBar from "./components/navigation/NavBar";

const queryClient = new QueryClient({
    defaultOptions: {
        queries : {
            refetchInterval: 100000,
            refetchOnWindowFocus: false
        }
    }
})

function App() {
  return (
    <div className="bg-gray-50 dark:bg-gray-900 h-screen">
        <QueryClientProvider client={queryClient}>
            <BrowserRouter>
                <NavBar />
                <AppRoutes />
            </BrowserRouter>
        </QueryClientProvider>
    </div>
  );
}

export default App;

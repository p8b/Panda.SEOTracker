import React, { Suspense } from 'react';
import MainLayout from './components/layout/mainLayout';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import AppContextProvider from './components/contexts/appContext';
import TrackedUrlPage from './pages/trackedUrl/trackedUrlPage';
import HistoryPage from './pages/historyPage';

const app: React.FC = () => {
    return (
        <AppContextProvider>
            <BrowserRouter>
                <MainLayout >
                    <Suspense fallback={<>...Loading</>}>
                        <Routes>
                            <Route path="/" element={<TrackedUrlPage />} />
                            <Route path="/History" element={<HistoryPage />} />
                            <Route path="*" element={<>Not Found</>} />
                        </Routes>
                    </Suspense>
                </MainLayout>
            </BrowserRouter>
        </AppContextProvider>
    );
};

export default app;
import React from 'react';
import { createRoot } from 'react-dom/client';
import App from './app';
import './styles/main.scss';

const container = document.getElementById('rootDiv');
const root = createRoot(container!);
root.render(<App />);
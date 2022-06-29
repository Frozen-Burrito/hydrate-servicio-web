import React from 'react'
import Navbar from '../Navbar/Navbar';

export default function Layout({ children }) {
  return (
    <div style={{ position: "relative" }}>
        <Navbar />
        {children}
    </div>
  );
}

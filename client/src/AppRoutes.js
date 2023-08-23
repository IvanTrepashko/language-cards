import React from 'react';
import {Route, Routes} from "react-router-dom";
import Home from "./pages/Home";
import Login from "./pages/Login";
import SignUp from "./pages/SignUp";

const AppRoutes = () => {
    return (
        <Routes>
            <Route path={'/'} element={<Home />} />
            <Route path={'/login'} element={<Login />} />
            <Route path={'/sign-up'} element={<SignUp />} />
        </Routes>
    );
};

export default AppRoutes;
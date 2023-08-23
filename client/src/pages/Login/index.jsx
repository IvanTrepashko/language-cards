import React from 'react';
import LoginForm from "./components/LoginForm";
import {useNavigate} from "react-router-dom";

function Login() {
    const navigate = useNavigate()

    return (
        <div>
            <LoginForm />
            <p>
                Sign up if you don't have an account
                <br />
                <button onClick={() => navigate('/sign-up')}>Sign up</button>
            </p>
        </div>
    );
}

export default Login;
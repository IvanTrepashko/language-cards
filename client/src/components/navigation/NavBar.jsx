import React from 'react';
import {Link} from "react-router-dom";

function NavBar() {
    return (
        <div>
            <ul className="flex">
                <li className="mr-6">
                    <Link to="/">Home</Link>
                </li>
                <li className="mr-6">
                    <Link to="/login">Login</Link>
                </li>
                <li className="mr-6">
                    <Link to="sign-up">Sign up</Link>
                </li>
            </ul>
        </div>
    );
}

export default NavBar;
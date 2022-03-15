import React from 'react'
import { Link } from 'react-router-dom';

export default function Botones() {
  return (
    <div>
        <Link className='btn transparent' to='/' style={{ textDecoration: 'none' }}>Log in</Link>
        <Link className='btn solid' to='/' style={{ textDecoration: 'none' }}>Sign up</Link>
    </div>
  )
}

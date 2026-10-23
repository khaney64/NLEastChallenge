import React, { Component } from 'react';
import { Navigate, Route, Routes } from 'react-router';
import { Home } from './components/Home';
import { Layout } from './components/Layout';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Routes>
          <Route path='/index.html' element={<Navigate to='/' replace />} />
          <Route path='/' element={<Home />} />
        </Routes>
      </Layout>
    );
  }
}

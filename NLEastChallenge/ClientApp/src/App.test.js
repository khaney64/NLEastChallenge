import React from 'react';
import ReactDOM from 'react-dom';
import { act, Simulate } from 'react-dom/test-utils';
import { MemoryRouter } from 'react-router-dom';
import App from './App';
import Standings from './components/Standings';

const normalDivisionData = {
  headers: ['Actual', 'Record', 'Streak', 'Greg'],
  rows: [
    [
      { team: 'ATL', teamName: 'ATL', record: '20-9', value: 0 },
      { team: 'Record', record: '20-9', value: 0 },
      { team: 'Streak', streak: 'W1', value: 0 },
      { team: 'PHI', teamName: 'PHI', winsGuess: 0, value: 5 }
    ]
  ],
  footers: ['', '', '', '5']
};

const horseshoesDivisionData = {
  headers: ['Actual', 'Record', 'Streak', 'Rank', 'Bonus', 'Both'],
  rows: [
    [
      { team: 'ATL', teamName: 'ATL', record: '20-9', value: 0 },
      { team: 'Record', record: '20-9', value: 0 },
      { team: 'Streak', streak: 'W1', value: 0 },
      { team: 'ATL', teamName: 'ATL', winsGuess: 0, value: 3, rankDistanceValue: 3, pairwiseBonusValue: 0 },
      { team: 'PHI', teamName: 'PHI', winsGuess: 0, value: 4, rankDistanceValue: 0, pairwiseBonusValue: 4 },
      { team: 'MIA', teamName: 'MIA', winsGuess: 0, value: 5, rankDistanceValue: 2, pairwiseBonusValue: 3 }
    ]
  ],
  footers: ['', '', '', '3', '4', '5']
};

beforeEach(() => {
  global.fetch = jest.fn((url) => {
    const path = url.toString();
    const data = path.startsWith('divisiondata?mode=hh')
      ? horseshoesDivisionData
      : path.startsWith('divisiondata')
        ? normalDivisionData
        : [];

    return Promise.resolve({
      json: () => Promise.resolve(data)
    });
  });
});

afterEach(() => {
  delete global.fetch;
});

it('renders without crashing', async () => {
  const div = document.createElement('div');

  await act(async () => {
    ReactDOM.render(
      <MemoryRouter>
        <App />
      </MemoryRouter>, div);
  });

  ReactDOM.unmountComponentAtNode(div);
});

it('switches standings scoring mode', async () => {
  const div = document.createElement('div');

  await act(async () => {
    ReactDOM.render(<Standings />, div);
  });

  const hhButton = div.querySelector('button[aria-label="Horseshoes and hand grenades scoring"]');

  await act(async () => {
    Simulate.click(hhButton);
  });

  expect(global.fetch).toHaveBeenCalledWith('divisiondata?mode=normal');
  expect(global.fetch).toHaveBeenCalledWith('divisiondata?mode=hh');

  ReactDOM.unmountComponentAtNode(div);
});

it('shows current scoring explanation', async () => {
  const div = document.createElement('div');

  await act(async () => {
    ReactDOM.render(<Standings />, div);
  });

  const scoringButton = div.querySelector('button[aria-label="Show scoring explanation"]');

  await act(async () => {
    Simulate.click(scoringButton);
  });

  expect(div.textContent).toContain('Normal: exact rank matches');

  ReactDOM.unmountComponentAtNode(div);
});

it('keeps normal scoring highlight green', async () => {
  const div = document.createElement('div');

  await act(async () => {
    ReactDOM.render(<Standings />, div);
  });

  expect(div.querySelectorAll('.score-normal')).toHaveLength(1);

  ReactDOM.unmountComponentAtNode(div);
});

it('shows horseshoes score reason colors', async () => {
  const div = document.createElement('div');

  await act(async () => {
    ReactDOM.render(<Standings />, div);
  });

  const hhButton = div.querySelector('button[aria-label="Horseshoes and hand grenades scoring"]');

  await act(async () => {
    Simulate.click(hhButton);
  });

  expect(div.querySelectorAll('.score-rank-distance')).toHaveLength(1);
  expect(div.querySelectorAll('.score-pairwise')).toHaveLength(1);
  expect(div.querySelectorAll('.score-mixed')).toHaveLength(1);

  ReactDOM.unmountComponentAtNode(div);
});

it('explains horseshoes score colors', async () => {
  const div = document.createElement('div');

  await act(async () => {
    ReactDOM.render(<Standings />, div);
  });

  const hhButton = div.querySelector('button[aria-label="Horseshoes and hand grenades scoring"]');
  const scoringButton = div.querySelector('button[aria-label="Show scoring explanation"]');

  await act(async () => {
    Simulate.click(hhButton);
  });

  await act(async () => {
    Simulate.click(scoringButton);
  });

  expect(div.textContent).toContain('green = close-enough rank points');
  expect(div.textContent).toContain('blue = order bonus only');
  expect(div.textContent).toContain('teal = both');

  ReactDOM.unmountComponentAtNode(div);
});

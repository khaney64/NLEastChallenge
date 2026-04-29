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

const flushPendingPromises = () => new Promise(resolve => setTimeout(resolve, 0));

const renderStandings = async (div) => {
  await act(async () => {
    ReactDOM.render(<Standings />, div);
    await flushPendingPromises();
  });
};

const clickAndFlush = async (element) => {
  await act(async () => {
    Simulate.click(element);
    await flushPendingPromises();
  });
};

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

  await renderStandings(div);

  const normalButton = div.querySelector('button[aria-label="Normal scoring"]');
  const hhButton = div.querySelector('button[aria-label="Horseshoes and hand grenades scoring"]');

  expect(normalButton.getAttribute('aria-pressed')).toBe('true');
  expect(hhButton.getAttribute('aria-pressed')).toBe('false');

  await clickAndFlush(hhButton);
  await act(async () => {
    await flushPendingPromises();
  });

  expect(global.fetch).toHaveBeenCalledWith('divisiondata?mode=normal');
  expect(global.fetch).toHaveBeenCalledWith('divisiondata?mode=hh');
  expect(div.querySelector('button[aria-label="Normal scoring"]').getAttribute('aria-pressed')).toBe('false');
  expect(div.querySelector('button[aria-label="Horseshoes and hand grenades scoring"]').getAttribute('aria-pressed')).toBe('true');

  ReactDOM.unmountComponentAtNode(div);
});

it('ignores stale standings responses after scoring mode changes', async () => {
  const div = document.createElement('div');
  let resolveNormal;

  global.fetch = jest.fn((url) => {
    const path = url.toString();
    if (path.startsWith('divisiondata?mode=normal')) {
      return new Promise(resolve => {
        resolveNormal = () => resolve({
          json: () => Promise.resolve(normalDivisionData)
        });
      });
    }

    return Promise.resolve({
      json: () => Promise.resolve(horseshoesDivisionData)
    });
  });

  let standings;
  await act(async () => {
    standings = ReactDOM.render(<Standings />, div);
  });

  await act(async () => {
    standings.changeMode('hh');
    await flushPendingPromises();
  });

  await act(async () => {
    resolveNormal();
    await flushPendingPromises();
  });

  expect(div.textContent).toContain('Rank');
  expect(div.textContent).toContain('Bonus');
  expect(div.textContent).not.toContain('Greg');

  ReactDOM.unmountComponentAtNode(div);
});

it('shows current scoring explanation', async () => {
  const div = document.createElement('div');

  await renderStandings(div);

  const scoringButton = div.querySelector('button[aria-label="Show scoring explanation"]');

  expect(scoringButton.getAttribute('title')).toBe('Show scoring explanation');

  await clickAndFlush(scoringButton);

  expect(scoringButton.getAttribute('aria-label')).toBe('Hide scoring explanation');
  expect(scoringButton.getAttribute('title')).toBe('Hide scoring explanation');
  expect(div.textContent).toContain('Normal: exact rank matches');

  ReactDOM.unmountComponentAtNode(div);
});

it('keeps normal scoring highlight green', async () => {
  const div = document.createElement('div');

  await renderStandings(div);

  expect(div.querySelectorAll('.score-normal')).toHaveLength(1);

  ReactDOM.unmountComponentAtNode(div);
});

it('keeps scoring controls outside table footer', async () => {
  const div = document.createElement('div');

  await renderStandings(div);

  expect(div.querySelector('tfoot .scoring-controls')).toBeNull();
  expect(div.querySelector('.table-responsive > .scoring-controls')).not.toBeNull();
  expect(div.querySelectorAll('tfoot td')).toHaveLength(normalDivisionData.footers.length);

  ReactDOM.unmountComponentAtNode(div);
});

it('shows horseshoes score reason colors', async () => {
  const div = document.createElement('div');

  await renderStandings(div);

  const hhButton = div.querySelector('button[aria-label="Horseshoes and hand grenades scoring"]');

  await clickAndFlush(hhButton);

  expect(div.querySelectorAll('.score-rank-distance')).toHaveLength(1);
  expect(div.querySelectorAll('.score-pairwise')).toHaveLength(1);
  expect(div.querySelectorAll('.score-mixed')).toHaveLength(1);

  ReactDOM.unmountComponentAtNode(div);
});

it('explains horseshoes score colors', async () => {
  const div = document.createElement('div');

  await renderStandings(div);

  const hhButton = div.querySelector('button[aria-label="Horseshoes and hand grenades scoring"]');
  const scoringButton = div.querySelector('button[aria-label="Show scoring explanation"]');

  await clickAndFlush(hhButton);

  await clickAndFlush(scoringButton);

  expect(div.textContent).toContain('green = close-enough rank points');
  expect(div.textContent).toContain('blue = order bonus only');
  expect(div.textContent).toContain('teal = both');

  ReactDOM.unmountComponentAtNode(div);
});

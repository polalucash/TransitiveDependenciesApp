import React, { Component } from 'react';

export class Home extends Component {
	static displayName = Home.name;

	render() {
		return (
			<div>				
				<h1> 
					<a target="_blank" href="https://snyk.io/">
						<img alt="Snyk Logo" src="https://s2-cdn.greenhouse.io/external_greenhouse_job_boards/logos/400/166/500/resized/Snyk-logo-colour-2020.png?1603191972" width="134" height="75"/>
					</a>
					Hello,
				</h1>

				<p>Welcome packages transitive Dependencies application, try the following.</p>
				<ul>
					<li>
						For the Tree representaion <a href={'package/react/latest'}>package/react/latest</a>
					</li>
					<li>
						For the Json representaion <a href={'api/package/react/latest'}>api/package/react/latest</a>
					</li>
					<li>
						For the original data <a href="https://registry.npmjs.org/react/latest">https://registry.npmjs.org/react/latest</a>
					</li>
				</ul>
			</div>
		);
	}
}

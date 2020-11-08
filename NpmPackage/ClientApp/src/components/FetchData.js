import React, { Component } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import TreeView from '@material-ui/lab/TreeView';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import ChevronRightIcon from '@material-ui/icons/ChevronRight';
import TreeItem from '@material-ui/lab/TreeItem';
import { Tooltip } from '@material-ui/core';
import Zoom from '@material-ui/core/Zoom';

export class FetchData extends Component {

	static displayName = FetchData.name;

	constructor(props) {
		super(props);
		const name = this.props.match.params.name;
		const version = this.props.match.params.version;
		this.state = { packages: [], name, version, loading: true };

		fetch(`api/package/${name}/${version}`)
			.then(response => response.json())
			.then(data => {
				this.setState({ packages: data, name, version, loading: false });
			});
	}

	render() {
		let contents = this.state.loading
			? <p><em>Loading...</em></p>
			: RecursiveTreeView(this.state.packages);

		return (
			<div>
				<a target="_blank" href="https://snyk.io/">
					<img alt="Snyk Logo" src="https://s2-cdn.greenhouse.io/external_greenhouse_job_boards/logos/400/166/500/resized/Snyk-logo-colour-2020.png?1603191972" width="134" height="75"/>
				</a>
				<h1>Transitive dependencies for the {`${this.state.name} ${this.state.version}`}</h1>
				{contents}
			</div>
		);
	}
}

function RecursiveTreeView(data) {
	const classes = makeStyles({
		root: {
			height: 110,
			flexGrow: 1,
			maxWidth: 400,
		},
	});

	const renderTree = (node) => (
		<Tooltip key={`Tooltip_${node.id}`}  title={node.description} TransitionComponent={Zoom} TransitionProps={{ timeout: 600 }} >
			<TreeItem key={`TreeItem_${node.id}`} nodeId={node.id} label={`${node.name} ${node.version}`}>
				{Array.isArray(node.children) ? node.children.map((child) => renderTree(child)) : null}
			</TreeItem>
		</Tooltip>
	);

	return (
		<TreeView
			className={classes.root}
			defaultCollapseIcon={<ExpandMoreIcon />}
			defaultExpanded={['root']}
			defaultExpandIcon={<ChevronRightIcon />}
		>
			{renderTree(data)}
		</TreeView>
	);
}


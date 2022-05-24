using Priority_Queue; // fast-priority queue
using System;
using System.Collections.Generic;

namespace TFE
{
    public class Dijkstra
    {
        // Fast priority queue forward.
        private FastPriorityQueue<State> _queue;
        // Fast priority queue backward.
        private FastPriorityQueue<State> _queueB; 
        // Graph sur lequel l'algorithme devra trouver le plus court chemin chemin.
        private Graph _graph;
        // Capacité maximum de la PQ avant qu'elle ne se redimensionne. Si sa valeur est par exemple de 200 et qu'elle est dépassée, alors elle se redimensionnera. 
        private int _priorityQueueMaxCapacity;
        // Valeur de l'appel actuelle de l'algorithme. S'il est appelé 3x, alors la valeur s'incrémentera au fur et amesure à chaque appel pour atteindre 3. 
        public int lastVisit = 0;
        // Représente le nombre total de noeuds ajoutés à la PQ.
        public int totalNumberOfnodes;
        // Représente le nombre total de noeuds parcouru et pris de la PQ.
        public int tookNodeNumber;

        public Dijkstra(Graph graph, int ppriorityQueueMaxCapacity = 1600)
        {
            _graph = graph;
            _priorityQueueMaxCapacity = ppriorityQueueMaxCapacity;
            _queue = new FastPriorityQueue<State>(_priorityQueueMaxCapacity);
            _queueB = new FastPriorityQueue<State>(_priorityQueueMaxCapacity);
        }

        /// <summary>
        ///     Fonction permettant d'extraire la valeur racine d'une des PQ.
        /// </summary>
        /// <param name="backwardPQ"> 
        ///     Booléen permettant de choisir à partir de quelle PQ le noeud doit-il être prit. 
        ///     Si la valeur est false, alors ce sera la PQ forward, sinon ce sera la backward.
        /// </param>
        /// <returns> Retourne l'état (l'objet "state") associé à la première valeur de la PQ sélectinnée. </returns>
        private CostWithNode _PopPriorityQueueHead(bool backwardPQ)
        {
            if (backwardPQ)
            {
                State s = _queueB.Dequeue();
                return new CostWithNode(s.totalCost, s);
            }
            else
            {
                State s = _queue.Dequeue();
                return new CostWithNode(s.totalCost, s);
            }
        }
        /// <summary>
        ///     Ajoute un état (ojbet "state") d'une des PQ.
        /// </summary>
        /// <param name="cost"> Coût auxquel l'état doit être ajouté. Il correspond au coût du chemin pour arriver jusuq'au noeud qu'il contient. </param>
        /// <param name="state"> Etat qui doit être ajouté et qui sera le noeud de la PQ. </param>
        /// <param name="backwardPQ"> 
        ///     Booléen permettant de choisir dans quelle PQ le noeud doit-il être ajouté. 
        ///     Si la valeur est false, alors ce sera la PQ forward, sinon ce sera la backward.
        /// </param>
        private void _AddNode(double cost, State state, bool backwardPQ)
        {
            // Incrémentation du nombre total de noeud ajouté dans les PQ.
            totalNumberOfnodes++;
            if (backwardPQ)
                _queueB.Enqueue(state, (float)cost);
            else
                _queue.Enqueue(state, (float)cost);
        }
        /// <summary>
        ///     Réinitialise les PQ pour qu'elles puissent être à nouveau utilisées lors du prochain appel de l'algorithme.
        /// </summary>
        private void _ClearQueue()
        {
            _queue.Clear();
            _queueB.Clear();
        }
        /// <summary>
        ///     Fonction ayant pour objectif de vérifier si l'une des deux PQ est vide. 
        ///     L'objectif est de savoir, via cette caractéristique, si l'algorithme a terminé d'explorer tous les vertices qu'il pouvait.
        /// </summary>
        /// <param name="backwardPQ"> 
        ///     Booléen permettant de choisir par rapport à quelle PQ la vérification doit être effectuée. 
        ///     Si la valeur est false, alors ce sera la PQ forward, sinon ce sera la backward.
        /// </param>
        /// <returns> Retourne un booléen indiquant si la PQ choisie est vide ou non. La valeur est true si elle est vide, false sinon. </returns>
        private bool _QueueIsEmpty(bool backwardPQ)
        {
            if (backwardPQ)
                return _queueB.Count == 0;
            else
                return _queue.Count == 0;
        }
        /// <summary>
        ///     Cette fonction à pour but de regrouper les conditions d'arrêt qui ne sont pas celle où l'algorithme trouve un plus court chemin.
        ///     Les moments où elles intervient sont celui ou un des noeuds d'entré n'existe pas dans le graphe ou pour vérifier si les PQ ne sont pas vide.
        /// </summary>
        /// <param name="sourceVertexID"> Vertex de départ d'où l'on veut trouver un chemin et d'où la recherche forward commence. </param>
        /// <param name="destinationVertexID"> Vertex de destination vers lequel on veut trouver un chemin et d'où la recherche backward commence. </param>
        /// <param name="message"></param>
        /// <returns> Retourne une clé-valeur où le coût est null et le state un objet vide. </returns>
        private KeyValuePair<double, State> _NoPathFound(int sourceVertexID, int destinationVertexID, string message)
        {
            //throw new InvalidOperationException($"{message} \n Noeud source -> {sourceVertexID}, noeud de destination -> {destinationVertexID}");
            return new KeyValuePair<double, State>();
        }
        /// <summary>
        ///     Fonction permettant de reconstruire le chemin complet entre le vertex source/destination à partir des état des deux recherches.
        /// </summary>
        /// <param name="forwardState"> Dernière état de la recherche forward. </param>
        /// <param name="backwardState"> Dernière état de la recherche backward. </param>
        /// <returns></returns>
        private State _BuildFinalPath(State forwardState, State backwardState)
        {
            backwardState.previousState = forwardState; // On fait une union entre le chemin du noeud "source -> v" (forwardState) et le premier  
            State tempState = backwardState;            //      noeud du second chemin (backwardState) vers le noeud de fin "v -> end".
            while (tempState.nextState != null)
            {
                tempState = tempState.nextState;
                tempState.previousState = backwardState;
                backwardState = tempState;
            }
            return tempState;
        }
        /// <summary>
        ///     Fonction gérant le calcul du coût pour atteindre le prochain vertex.
        /// </summary>
        /// <param name="totalCost"> Coût total pour arriver jusqu'au vertex traité actuellement. </param>
        /// <param name="costToNextVertex"> Coût pour atteindre le prochain vertex (coût de l'arête). </param>
        /// <returns></returns>
        private double _CostEvaluation(double totalCost,
                                       double costToNextVertex)
        {
            return totalCost + // Somme du coût des vertices précédements visités, soit du chemin total.   
                   costToNextVertex // Le coût pour rejoindre le prochain vertex.
                   ;
        }
        /// <summary>
        ///     Fonction implémentant l'algorithme de Dijkstra bidirectionnelle.
        /// </summary>
        /// <param name="sourceVertexID"> Identifiant du vertex source, soit celui d'où l'on part et à partir duquel on veut trouver un chemin. L'id est la valeur "source" venant du format OSM. </param>
        /// <param name="destinationVertexID"> Identifiant du vertex de destination, soit celui vers lequel on veut trouver un chemin. L'id est aussi la valeur "source" venant du format OSM. </param>
        /// <returns> 
        ///     Retourne le dernier état (l'objet state) qui constitue le chemin et à partir duquel, en parcourant les variables "previousState" de chacun d'eux, il possible de trouver l'nesmble des vertices. 
        ///     Le résultat est une clé valeur avec en premier lieu le coût total pour atteindre le dernier état (et donc atteindre le vertex de destination) et en second lieu est l'état y correspondant.
        ///     Si le chemin n'est pas trouvé alors une clé valeur de zéro et un objet vide sera renvoyé.
        /// </returns>
        public KeyValuePair<double, State> ComputeShortestPath(int sourceVertexID, int destinationVertexID)
        {
            Dictionary<int, State> bestPathNodesForward = new Dictionary<int, State>();
            Dictionary<int, State> bestPathNodesBackward = new Dictionary<int, State>();
            if (!_graph.VertexExist(sourceVertexID) || !_graph.VertexExist(destinationVertexID))
                // Arrête si le noeud de départ ou celui recherché n'existe pas
                return _NoPathFound(sourceVertexID, destinationVertexID, Messages.VertexDontExist); 
            lastVisit++;
            // Réinitialisation des PQ.
            _ClearQueue();
            // Ajout des noeuds de départ dans chacunes des PQ.
            _AddNode(0, new State(0, _graph.GetVertex(sourceVertexID)), false);
            _AddNode(0, new State(0, _graph.GetVertex(destinationVertexID)), true);  
            // Initialisation de la variable permettant de savoir quand s'arrêter à l'infini.
            double mu = double.PositiveInfinity;
            // Variable des dernier états mis à jour correspondant au meilleur chemin des deux PQ.
            State lastBestStateForward = null;
            State lastBestStateBackward = null;
            tookNodeNumber = 0;
            totalNumberOfnodes = 0;
            //------------------------------------------------------------------------------------------------- Début de l'algorithme.
            // Vérification si les PQ sont vide pour savoir s'il faut continuer ou s'arrêter.
            while (!_QueueIsEmpty(true) && !_QueueIsEmpty(false))
            {
                CostWithNode headForward = _PopPriorityQueueHead(false);
                CostWithNode headBackward = _PopPriorityQueueHead(true);
                tookNodeNumber++;
                // Condition d'arrêt.
                if (headForward.cost + headBackward.cost >= mu)  
                {
                    // On retourne mu ainsi que le chemin final des deux recherches contenu dans les deux états.
                    return new KeyValuePair<double, State>(mu, _BuildFinalPath(lastBestStateForward, lastBestStateBackward));
                }
                // Vérification que les vertices n'ont pas déjà été visités. 
                bool forwardVertexHasBeenVisited = headForward.state.vertex.lastVisitForward == lastVisit;    
                bool backwardVertexHasBeenVisited = headBackward.state.vertex.lastVisitBackward == lastVisit; 
                // Actualisation de leur valeur "lastVisit" à celle de l'algorithme pour les marquer comme ayant déjà été visités.
                headForward.state.vertex.lastVisitForward = lastVisit;
                headBackward.state.vertex.lastVisitBackward = lastVisit;
                // Ajout du meilleur state pour un vertex déjà parcouru.
                if (!forwardVertexHasBeenVisited) 
                    bestPathNodesForward.Add(headForward.state.getVertexID, headForward.state);
                if (!backwardVertexHasBeenVisited)
                    bestPathNodesBackward.Add(headBackward.state.getVertexID, headBackward.state);
                // Recherche forward pour trouver le chemin dans le sens traditionnel (forward).
                // Vérification que le vertex de la recherche forward n'ai pas déjà été visité.
                if (!forwardVertexHasBeenVisited)
                {                   
                    foreach (Edge nextEdge in _graph.GetNextEdges(headForward.state.vertex.id, lastVisit)) 
                    {
                        double cost = _CostEvaluation(headForward.cost, nextEdge.cost);
                        _AddNode(cost, new State(cost, nextEdge.targetVertex, headForward.state), false);
                        // Vérification d'une part que le vertex de la recherche backward n'ai pas déjà été visité (et donc qu'un croisement entre les deux recherches ne vient pas de se faire,
                        //  et d'autre part que le coût pour atteindre ce vertex dans la recherche forward plus celui pour l'atteindre dans la recherche backward est inférieur à mu (pouvant qu'un
                        //  chemin plus court a été trouvé).
                        if (nextEdge.targetVertex.lastVisitBackward == lastVisit && cost + bestPathNodesBackward[nextEdge.targetVertex.id].totalCost < mu)
                        {
                            // Actualisation de mu si un chemin plus court a été trouvé.
                            mu = cost + bestPathNodesBackward[nextEdge.targetVertex.id].totalCost;
                            // Actualisation du dernier meilleur état trouvé de la recherche forward.
                            lastBestStateForward = headForward.state;
                            // Actualisation du dernier meilleur état trouvé de la recherche backward.
                            lastBestStateBackward = bestPathNodesBackward[nextEdge.targetVertex.id];
                        }
                    }
                }               
                //-------------------------------------------------------------------------------------------------------
                // Recherche backward pour trouver le chemin dans le sens inverse (backward) suivant la même logique que la forward mais adpaté au sens inverse.
                if (!backwardVertexHasBeenVisited)
                {
                    // Ici au lieu de récupérer les arêtes menant au vertices suivant on prend celle menant au vertices précédents (graphe inverse).
                    foreach (Edge nextEdge in _graph.GetNextReverseEdges(headBackward.state.vertex.id, lastVisit))
                    {
                        double cost = _CostEvaluation(headBackward.cost, nextEdge.cost);
                        _AddNode(cost, new State(cost, nextEdge.sourceVertex, null, headBackward.state), true);
                        if (nextEdge.sourceVertex.lastVisitForward == lastVisit && cost + bestPathNodesForward[nextEdge.sourceVertex.id].totalCost < mu)
                        {
                            mu = cost + bestPathNodesForward[nextEdge.sourceVertex.id].totalCost;
                            lastBestStateForward = bestPathNodesForward[nextEdge.sourceVertex.id];
                            lastBestStateBackward = headBackward.state;
                        }
                    }
                }
            }
            return _NoPathFound(sourceVertexID, destinationVertexID, Messages.NoPathFound);
        }
    }
    public class State : FastPriorityQueueNode
    {
        // Coût total (smme du cpupt des arêtes) pour atteindre le vertex qui sera ajouté à cet état.
        public double totalCost { get; } 
        public Vertex vertex { get; }
        // Etat précédant celui-ci qui permet de retrouvé la succession de vertices qui constituent le chemin pour la recherche forward. 
        public State previousState { get; set; }
        // Etat précédant celui-ci qui permet de retrouvé la succession de vertices qui constituent le chemin pour la recherche backward. 
        public State nextState { get; set; }
        // Récupération de l'identifiant OSM du vertex.
        public int getVertexID
        {
            get { return vertex.id;  }
        }

        public State(double ptotalCost,
                    Vertex pvertex,
                    State ppreviousState = null,
                    State pnextState = null
                    )
        {
            totalCost = ptotalCost;
            vertex = pvertex;
            previousState = ppreviousState;
            nextState = pnextState;
        }
    }
    /// <summary>
    ///     Structure de données permettant de simplifier et de rendre plus lisible le travail sur les coûts et les états.
    /// </summary>
    internal struct CostWithNode
    {
        public double cost { get; }
        public State state { get; }

        public CostWithNode(double pcost, State pstate)
        {
            cost = pcost;
            state = pstate;
        }
    }

}
